using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectWGameManager : NetworkBehaviour {
    public ProjectWNetworkManager networkManager;
    public float respawnTime = 5;
    public float timeLimit = 20 * 60 * 60; // 20 minutes
    public float gameTime;
    public Transform[] startPositions;
	public GameObject spectatorPrefab;
    public int botCount = 0;
    public bool friendlyFire;
	public bool useSettingsOverride;
	public Team[] teams;
    public Scoreboard scoreBoard;
	public SceneHolder scenesToLoad;
	public ColorHolder teamColors;
    private GameMode gameMode;
    private GameObject[] classPrefabs;
    private bool isGameOver;

    public PrefabHolder classPrefabHolder;
	public PrefabHolder gamemodePrefabHolder;
    

    [System.Serializable]
	public struct Team
	{
		public string teamName;
        public int teamIndex;
        public Color teamColor;
        public int teamSprite;
	}
    public class Winner
    {
        public bool exists = true;
        public string winnerName;
        public string winnerScore;
        public Color winnerColor;
    }
    
    public void AddDeath(GameObject player, string playerid) {
        if (!isServer) return;
        int lastHitPlayer = player.GetComponent<PlayerStats>().lastHitPlayerId;
        int playerId = player.GetComponent<PlayerInput>().GetPlayerId();

        // Update deaths
        scoreBoard.UpdateScore(playerId, 0, 0, 1);

        if (lastHitPlayer != playerId && lastHitPlayer != 0)
        { // 0 means no hit, dont want to kill ourselves
            // Update kills
            scoreBoard.UpdateScore(lastHitPlayer, 1, 0, 0);
        }


        //print("Adding death as player: " + playerid);
        //table.Add(playerid, true);
        StartCoroutine(cRespawnPlayer(player));
    }
    public IEnumerator cRespawnPlayer(GameObject player) {
        //print("Starting respawn player");
        yield return new WaitForSeconds(respawnTime);
		print("Now respawning player");
		PlayerStats oldP;
		PlayerGUI oldPG = null;
		// if it is -1, they are joining the ffa team
        // if it is -2, they want to be a spectator, so no respawn
		if ((oldP = player.GetComponent<PlayerStats> ()) != null && (oldPG = player.GetComponent<PlayerGUI> ()) != null && oldPG.desiredTeamIndex != -2) {
			NetworkConnection connection = oldP.connectionToClient;
			Transform startPosition = GetStartPosition ();
			GameObject newPlayer = Instantiate (classPrefabs [oldPG.desiredPlayerClass], startPosition.position, startPosition.rotation);
			newPlayer.name = oldP.playerName;
			PlayerStats newP = newPlayer.GetComponent<PlayerStats> ();
			newP.playerName = oldPG.desiredPlayerName;
			newP.classIndex = oldPG.desiredPlayerClass;
			newP.teamIndex = teams.Length > 1 ? oldPG.desiredTeamIndex : -1;
			newP.teamColor = teams [teams.Length > 1 ? newP.teamIndex : 0].teamColor;
			PlayerGUI newPG = newPlayer.GetComponent<PlayerGUI> ();
			newPG.desiredPlayerName = oldPG.desiredPlayerName;
			newPG.desiredPlayerClass = oldPG.desiredPlayerClass;
			newPG.desiredTeamIndex = oldPG.desiredTeamIndex;
			// If not a bot, move connection to a new thing
			int botId;
			if ((botId = player.GetComponent<PlayerInput> ().getBot ()) != -1) {
				newPlayer.GetComponent<PlayerInput> ().SendMessage ("setBot", botId);
				NetworkServer.Spawn (newPlayer);
			} else {
				NetworkServer.Spawn (newPlayer);
				NetworkServer.ReplacePlayerForConnection (connection, newPlayer, 0);
				AssignPlayerId (newPlayer);
			}
		} else {
			//print (oldPG.desiredTeamIndex + " desired team index");
		}
       
        yield return new WaitForSeconds(5);
        //print("destroying corpse");
		oldP.despawnCorpse ();
    }
    void Start() {
		networkManager = GameObject.FindObjectOfType<ProjectWNetworkManager> ();

		// If we created the game ourselves
		if (networkManager.teamItems.Length > 0 && !useSettingsOverride) {
			int teamCount = networkManager.teamItems.Length;
			teams = networkManager.teamItems;
			// GameMode & options
			int gamemodeType = networkManager.gameModeSelect;
			// People may forget the time limit in their games
			if (networkManager.gamemodeOptions[0].optionName == "Time Limit") {
				gameTime = networkManager.gamemodeOptions [0].value * 60; // Time is passed in minutes, we use seconds here
			} else {
				gameTime = timeLimit; // TODO should we just make this infinite?
			}

			switch (gamemodeType) {
				// README if you want to add a gamemode, you MUST update this method with the specific gamemode class
			case 0: 
				gameMode = this.gameObject.AddComponent<GameMode_Deathmatch> ();
				GameMode_Deathmatch dm = (GameMode_Deathmatch)gameMode;
				dm.gameOptions = networkManager.gamemodeOptions;
				break;
			default:
				gameMode = null;
				break;
			}


		} else {
			if (!(gameMode = this.GetComponent<GameMode>()))
			{
				Debug.LogWarning("Unable to find a gamemode. This game will only end from timing out");
			}
			gameTime = timeLimit;

			networkManager.gameObject.GetComponent<NetworkManagerHUD> ().gameObject.SetActive (true);
		}


		

        classPrefabs = classPrefabHolder.prefabs;
        
       
        if (isServer) {
            for (int i = 0; i > teams.Length; i++)
            {
                teams[i].teamIndex = i;
            }

            for (int i = 0; i < botCount; i++) {
                Transform startPosition = GetStartPosition();
                int classIndex = Random.Range(0, classPrefabs.Length - 1);
                GameObject spawn = Instantiate(classPrefabs[classIndex], startPosition.position, startPosition.rotation);
                spawn.SendMessage("setBot", i + 1);
                PlayerStats stats = spawn.GetComponent<PlayerStats>();
                int teamIndex = Random.Range(0, teams.Length);
                stats.teamIndex = teams.Length > 1 ? teamIndex : -1;
                stats.teamColor = teams[teams.Length > 1 ? teamIndex : 0].teamColor;
                stats.classIndex = classIndex;
                stats.playerName = classPrefabs[classIndex].name + " " + i;

                PlayerGUI newPG = spawn.GetComponent<PlayerGUI>();
                newPG.desiredPlayerName = stats.playerName;
                newPG.desiredPlayerClass = stats.classIndex;
                newPG.desiredTeamIndex = stats.teamIndex;
                spawn.name = stats.playerName;

                NetworkServer.Spawn(spawn);
            }
        } else {
            // this should not do anything, but should still display values such as time remaining and teams
            this.gameObject.SetActive(false);
        }
       
    }

    // Update is called once per frame
    void Update() {
        gameTime -= Time.deltaTime;
        if (isGameOver)
        {
            if (gameTime < -3.5f)
            {
                GameReset();
				int randomRange = Random.Range (0, scenesToLoad.scenes.Length - 1);
				print ("Changing scene to scene: " + randomRange + " out of " + scenesToLoad.scenes.Length);
                networkManager.ServerChangeScene(scenesToLoad.scenes[randomRange].name);
            }
            
        } else
        {
            Winner winner = CheckWinCondition();
            if (winner.exists)
            {
                GameOver(winner);
                gameTime = 0;
                isGameOver = true;
            }
            
        }
        
    }
    Transform GetStartPosition() {
        return startPositions[UnityEngine.Random.Range(0, startPositions.Length - 1)];
    }

	public void SpawnPlayer(int classIndex, int teamIndex, string playerName, GameObject source) {
        NetworkConnection connection = source.GetComponent<NetworkBehaviour>().connectionToClient;
        Transform startPosition = GetStartPosition();
        GameObject newPlayer = Instantiate(classPrefabs[classIndex], startPosition.position, startPosition.rotation);
		newPlayer.name = playerName;
		PlayerStats p = newPlayer.GetComponent<PlayerStats> ();
		p.teamIndex = teams.Length > 1 ? teamIndex : -1;
        p.teamColor = teams[teams.Length > 1 ? teamIndex : 0].teamColor;
        p.playerName = playerName;
        p.classIndex = classIndex;
        PlayerGUI newPG = newPlayer.GetComponent<PlayerGUI>();
        newPG.desiredPlayerName = playerName;
        newPG.desiredPlayerClass = classIndex;
        newPG.desiredTeamIndex = teamIndex;
        NetworkServer.Spawn(newPlayer);
        // If not a bot, move connection to a new thing
        NetworkServer.ReplacePlayerForConnection(connection, newPlayer, 0);
        AssignPlayerId(newPlayer);
        GameObject.Destroy(source);
    }
	public void SpawnSpectator(GameObject source) {
		source.GetComponent<PlayerNetworking> ().initializeCameras (false);

		NetworkConnection connection = source.GetComponent<NetworkBehaviour>().connectionToClient;
		Transform startPosition = GetStartPosition();
		GameObject newPlayer = Instantiate(spectatorPrefab, startPosition.position, startPosition.rotation);

		NetworkServer.Spawn(newPlayer);

		NetworkServer.ReplacePlayerForConnection(connection, newPlayer, 0);
        source.SendMessage("CmdDeath");
        source.SendMessage("StopGUI");
        //GameObject.Destroy(source);
    }
    public void GameOver(Winner winner)
    {
        foreach (PlayerNetworking p in GameObject.FindObjectsOfType<PlayerNetworking>())
        {
            p.RpcGameOver(winner);
        }
        foreach (Spectator p in GameObject.FindObjectsOfType<Spectator>())
        {
            p.RpcGameOver(winner);
        }
    }
    public void GameReset()
    {
        Time.timeScale = 1;
        foreach (PlayerNetworking p in GameObject.FindObjectsOfType<PlayerNetworking>())
        {
            p.RpcGameReset();
        }
        foreach (Spectator p in GameObject.FindObjectsOfType<Spectator>())
        {
            p.RpcGameReset();
        }
    }
    /**
     * Checks to see if the game is over
     * If it is not over, winner is returned null
     * Otherwise, we return the winner with the data about who won
     * 
     */
    public Winner CheckWinCondition()
    {
        // Timeout condition
        if (gameTime - Time.deltaTime <= 0)
        {
            gameTime = 0;
            Winner timeWinner = new Winner
            {
                exists = true
            };
            timeWinner.winnerName = "Nobody";
            return timeWinner;
        }
        if (gameMode != null)
        {
            return gameMode.checkWinCondition();
        }

        // Otheriwise, nobody win
        Winner notWon = new Winner
        {
            exists = false
        };
        return notWon;
    }


    public void AssignPlayerId(GameObject target)
    {
        NetworkConnection connectionToClient = target.GetComponent<NetworkBehaviour>().connectionToClient;
        //print(connectionToClient.connectionId);

        target.GetComponent<PlayerInput>().setPlayerId(connectionToClient.connectionId + 1);

    }

	private T CopyComponent<T>(Team original, GameObject destination) where T : Component
	{
		System.Type type = original.GetType ();
		Component copy = destination.AddComponent (type);
		System.Reflection.FieldInfo[] fields = type.GetFields ();
		foreach (System.Reflection.FieldInfo field in fields) {
			field.SetValue (copy, field.GetValue (original));
		}
		return copy as T;
	}

}
