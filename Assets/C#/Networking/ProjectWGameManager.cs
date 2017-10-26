using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectWGameManager : NetworkBehaviour {
    public ProjectWNetworkManager networkManager;
    public float respawnTime = 5;
    public float timeLimit = 20 * 60 * 60; // 20 minutes
    public Transform[] startPositions;
    public GameObject playerPrefab;
    public GameObject[] botPrefabs;
    public int botCount = 0;
    
    
    public void AddDeath(GameObject player, string playerid) {
        //print("Adding death as player: " + playerid);
        //table.Add(playerid, true);
        StartCoroutine(cRespawnPlayer(player.GetComponent<PlayerStats>()));
    }
    public IEnumerator cRespawnPlayer(PlayerStats player) {
        //print("Starting respawn player");
        yield return new WaitForSeconds(respawnTime);
        //print("Now respawning player");
        NetworkConnection connection = player.connectionToClient;
        Transform startPosition = GetStartPosition();
        GameObject newPlayer = Instantiate(playerPrefab, startPosition.position, startPosition.rotation);
        NetworkServer.Spawn(newPlayer);
        // If not a bot, move connection to a new thing
        if (player.GetComponent<PlayerInput>().isBot()) {
            newPlayer.GetComponent<PlayerInput>().SendMessage("setBot");
        } else {
            NetworkServer.ReplacePlayerForConnection(connection, newPlayer, 0);
        }
        yield return new WaitForSeconds(15);
        print("Now deleting player");
        Destroy(player.gameObject);
    }
    void Start() {
        //table = new Hashtable();
        if (isServer) {
            if (playerPrefab == null) {
                playerPrefab = networkManager.playerPrefab;
            }

            for (int i = 0; i < botCount; i++) {
                Transform startPosition = GetStartPosition();
                GameObject spawn = Instantiate(botPrefabs[Random.Range(0, botPrefabs.Length - 1)], startPosition.position, startPosition.rotation);
                spawn.SendMessage("setBot");
                NetworkServer.Spawn(spawn);
            }
        } else {
            this.gameObject.SetActive(false);
        }
       
    }

    // Update is called once per frame
    void Update() {
        if (timeLimit - Time.deltaTime <= 0) {
            timeLimit = 0;
            // Idk, do something here?
        } else {
            timeLimit -= Time.deltaTime;
        }
    }
    Transform GetStartPosition() {
        return startPositions[UnityEngine.Random.Range(0, startPositions.Length - 1)];
    }
   
}
