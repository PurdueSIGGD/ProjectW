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
    private GameObject[] classPrefabs;
	public GameObject spectatorPrefab;
    public int botCount = 0;
    public bool friendlyFire;
	public Team[] teams;
    private bool isGameOver;

    public PrefabHolder classPrefabHolder;


    [System.Serializable]
	public struct Team
	{
		public string teamName;
        public int teamIndex;
        public Color teamColor;
	}
    [System.Serializable]
    public struct Winner
    {
        public string winnerName;
        public string winnerScore;
        public Color winnerColor;
    }
    
    public void AddDeath(GameObject player, string playerid) {
        if (!isServer) return;
        //print("Adding death as player: " + playerid);
        //table.Add(playerid, true);
        StartCoroutine(cRespawnPlayer(player));
    }
    public IEnumerator cRespawnPlayer(GameObject player) {
        //print("Starting respawn player");
        yield return new WaitForSeconds(respawnTime);
		print("Now respawning player");
		PlayerStats oldP;
        PlayerGUI oldPG;
		// if it is -1, they want to be a spectator, so no respawn
		if ((oldP = player.GetComponent<PlayerStats>()) != null && (oldPG = player.GetComponent<PlayerGUI>()) != null && oldPG.desiredTeamIndex != -1) {
			NetworkConnection connection = oldP.connectionToClient;
			Transform startPosition = GetStartPosition();
            GameObject newPlayer = Instantiate(classPrefabs[oldPG.desiredPlayerClass], startPosition.position, startPosition.rotation);
            newPlayer.name = oldP.playerName;
            PlayerStats newP = newPlayer.GetComponent<PlayerStats> ();
            newP.playerName = oldPG.desiredPlayerName;
            newP.classIndex = oldPG.desiredPlayerClass;
            newP.teamIndex = oldPG.desiredTeamIndex;
            newP.teamColor = teams[newP.teamIndex].teamColor;
            PlayerGUI newPG = newPlayer.GetComponent<PlayerGUI>();
            newPG.desiredPlayerName = oldPG.desiredPlayerName;
            newPG.desiredPlayerClass = oldPG.desiredPlayerClass;
            newPG.desiredTeamIndex = oldPG.desiredTeamIndex;
            NetworkServer.Spawn(newPlayer);
            // If not a bot, move connection to a new thing
            if (player.GetComponent<PlayerInput>().isBot()) {
				newPlayer.GetComponent<PlayerInput>().SendMessage("setBot");
			} else {
				NetworkServer.ReplacePlayerForConnection(connection, newPlayer, 0);
            }
        }
       
        yield return new WaitForSeconds(5);
        //print("destroying corpse");
		oldP.despawnCorpse ();
    }
    void Start() {
        classPrefabs = classPrefabHolder.prefabs;
        gameTime = timeLimit;
        if (isServer) {
            for (int i = 0; i > teams.Length; i++)
            {
                teams[i].teamIndex = i;
            }

            for (int i = 0; i < botCount; i++) {
                Transform startPosition = GetStartPosition();
                int classIndex = Random.Range(0, classPrefabs.Length - 1);
                GameObject spawn = Instantiate(classPrefabs[classIndex], startPosition.position, startPosition.rotation);
                spawn.SendMessage("setBot");
                NetworkServer.Spawn(spawn);
                PlayerStats stats = spawn.GetComponent<PlayerStats>();
                int teamIndex = Random.Range(0, teams.Length);
                print(teams.Length + " team index: " + teamIndex);
                stats.teamIndex = teamIndex;
                stats.teamColor = teams[teamIndex].teamColor;
                stats.classIndex = classIndex;
                stats.playerName = classPrefabs[classIndex].name + " " + i;

                PlayerGUI newPG = spawn.GetComponent<PlayerGUI>();
                newPG.desiredPlayerName = stats.playerName;
                newPG.desiredPlayerClass = stats.classIndex;
                newPG.desiredTeamIndex = stats.teamIndex;
                spawn.name = stats.playerName;
            }
        } else {
            // this should not do anything, but should still display values such as time remaining and teams
            this.gameObject.SetActive(false);
        }
       
    }

    // Update is called once per frame
    void Update() {
        if (gameTime - Time.deltaTime <= 0) {
            if (isGameOver)
            {
                gameTime -= Time.deltaTime;
                if (gameTime < -5)
                {
                    // TODO new map
                }
            } else
            {
                gameTime = 0;
                Winner timeWinner = new Winner();
                // Timeout
                timeWinner.winnerName = "Nobody";
                GameOver(timeWinner);
                isGameOver = true;
            }
           
        } else {
            gameTime -= Time.deltaTime;
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
		p.teamIndex = teamIndex;
        p.teamColor = teams[teamIndex].teamColor;
        p.playerName = playerName;
        p.classIndex = classIndex;
        PlayerGUI newPG = newPlayer.GetComponent<PlayerGUI>();
        newPG.desiredPlayerName = playerName;
        newPG.desiredPlayerClass = classIndex;
        newPG.desiredTeamIndex = teamIndex;
        NetworkServer.Spawn(newPlayer);
        // If not a bot, move connection to a new thing
        NetworkServer.ReplacePlayerForConnection(connection, newPlayer, 0);
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
        ArrayList players = new ArrayList();
        foreach (PlayerStats p in GameObject.FindObjectsOfType<PlayerStats>())
        {
            players.Add(p.gameObject);
        }
        foreach (Spectator p in GameObject.FindObjectsOfType<Spectator>())
        {
            players.Add(p.gameObject);
        }

        foreach (Object o in players.ToArray())
        {
            ((GameObject)o).SendMessage("RpcGameOver", winner);
        }
    }

}
