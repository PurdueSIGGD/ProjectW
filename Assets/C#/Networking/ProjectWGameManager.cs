﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectWGameManager : NetworkBehaviour {
    public ProjectWNetworkManager networkManager;
    public float respawnTime = 5;
    public float timeLimit = 20 * 60 * 60; // 20 minutes
    public float gameTime;
	public Transform startPositionParent;
    private Transform[] startPositions;
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
        public int teamColorIndex;
        public int teamSprite;
	}
	[System.Serializable]
	public struct Bot
	{
		public string botName; 			// If blank, will be "Flame Player 1" etc
		public int botTeam; 			// -1 for auto assign
		public int botDifficulty; 		// TODO
		public int botType;				// -1 for random
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
        int lastHitWeapon = player.GetComponent<PlayerStats>().lastHitWeaponType;
        int playerId = player.GetComponent<PlayerInput>().GetPlayerId();
        string victimText = player.name;
        int victimTeam = player.GetComponent<PlayerStats>().teamIndex;
        // Update deaths
        scoreBoard.UpdateScore(playerId, 0, 0, 1);

		if (lastHitPlayer != playerId && lastHitPlayer != 0) { // 0 means no hit, dont want to kill ourselves
			// Update kills
			scoreBoard.UpdateScore (lastHitPlayer, 1, 0, 0);
		} else {
			// If you killed yourself, oopsie daysies you screwed up
			scoreBoard.UpdateScore (playerId, -1, 0, 0);
            		lastHitPlayer = -100; // Signify that nobody hit us, dying of natural causes
		}

        AddKillfeedItem(lastHitPlayer, lastHitWeapon, playerId);

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
			newPlayer.name = oldPG.desiredPlayerName;
			PlayerStats newP = newPlayer.GetComponent<PlayerStats> ();
			newP.playerName = oldPG.desiredPlayerName;
			newP.classIndex = oldPG.desiredPlayerClass;
			newP.teamIndex = teams.Length > 1 ? oldPG.desiredTeamIndex : -1;
			newP.teamColor = teams [teams.Length > 1 ? newP.teamIndex : 0].teamColor;
            newP.teamColorIndex = teams[teams.Length > 1 ? newP.teamIndex : 0].teamColorIndex;
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
       
        //yield return new WaitForSeconds(5);
        //print("destroying corpse");
		oldP.despawnCorpse ();
    }
    void Start() {
		networkManager = GameObject.FindObjectOfType<ProjectWNetworkManager> ();

		startPositions = startPositionParent.GetComponentsInChildren<Transform>();
		classPrefabs = classPrefabHolder.prefabs;

		// If we created the game ourselves
		if (networkManager.teamItems.Length > 0 && !useSettingsOverride) {
			int teamCount = networkManager.teamItems.Length;
			teams = networkManager.teamItems;
			// GameMode & options
			int gamemodeType = networkManager.gameModeSelect;
            // Game time
			// People may forget the time limit in their games
			if (networkManager.gamemodeOptions[0].optionName == "Time Limit (m)") {
				gameTime = networkManager.gamemodeOptions [0].value * 60; // Time is passed in minutes, we use seconds here
			} else {
                print("Using default time limit");
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


		

        
       
        if (isServer) {
            for (int i = 0; i > teams.Length; i++)
            {
                teams[i].teamIndex = i;
            }
			int botCount = 0; 
			foreach (Bot bot in networkManager.botItems) {
				Transform startPosition = GetStartPosition();
				int classIndex = ((bot.botType == -1) ? Random.Range(0, classPrefabs.Length - 1) : bot.botType);
				//print (classIndex + " " + classPrefabs.Length);
				int teamIndex = bot.botTeam != -1 ? bot.botTeam : Random.Range(0, teams.Length);
				string spawnName = bot.botName == "" ? classPrefabs [classIndex].name + " " + (botCount + 1) : bot.botName;
				//print (startPosition.position + " " + startPosition.rotation);
				GameObject spawn = GameObject.Instantiate(classPrefabs[classIndex], startPosition.position, startPosition.rotation);
				spawn.SendMessage("setBot", botCount + 1);
				PlayerStats stats = spawn.GetComponent<PlayerStats>();
				stats.teamIndex = teams.Length > 1 ? teamIndex : -1;
				stats.teamColor = teams[teams.Length > 1 ? teamIndex : 0].teamColor;
                stats.teamColorIndex = teams[teams.Length > 1 ? teamIndex : 0].teamColorIndex;
                stats.classIndex = classIndex;
				stats.playerName = spawnName;

				PlayerGUI newPG = spawn.GetComponent<PlayerGUI>();
				newPG.desiredPlayerName = stats.playerName;
				newPG.desiredPlayerClass = stats.classIndex;
				newPG.desiredTeamIndex = stats.teamIndex;
				spawn.name = stats.playerName;

				NetworkServer.Spawn(spawn);

				botCount++;
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
				print ("Changing scene to scene: " + (randomRange + 1) + " out of " + scenesToLoad.scenes.Length);
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
        p.teamColorIndex = teams[teams.Length > 1 ? teamIndex : 0].teamColorIndex;
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

		// TODO find a better workaround
		// We damage the player for 0 here because there's a bug where the server crashes if they aren't
		// I don't really understand what the hell it means, but this should fix it
		//p.Hit(new HitArguments(p.gameObject, p.gameObject));
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
    public void AddKillfeedItem(int killer, int weaponIndex, int victim) {
        foreach (PlayerNetworking p in GameObject.FindObjectsOfType<PlayerNetworking>()) {
            p.RpcAddKillfeedItem(killer, weaponIndex, victim);
        }
        foreach (Spectator p in GameObject.FindObjectsOfType<Spectator>()) {
            p.RpcAddKillfeedItem(killer, weaponIndex, victim);
        }
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
