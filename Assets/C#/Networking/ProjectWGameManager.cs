using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectWGameManager : NetworkBehaviour {
    public ProjectWNetworkManager networkManager;
    public float respawnTime = 5;
    public float timeLimit = 20 * 60 * 60; // 20 minutes
    public Transform[] startPositions;
    public GameObject[] classPrefabs;
    public int botCount = 0;
    
    
    public void AddDeath(GameObject player, string playerid) {
        if (!isServer) return;
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
        GameObject newPlayer = Instantiate(classPrefabs[player.GetComponent<PlayerGUI>().classIndex], startPosition.position, startPosition.rotation);
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
        if (isServer) {

            for (int i = 0; i < botCount; i++) {
                Transform startPosition = GetStartPosition();
                GameObject spawn = Instantiate(classPrefabs[Random.Range(0, classPrefabs.Length - 1)], startPosition.position, startPosition.rotation);
                spawn.SendMessage("setBot");
                NetworkServer.Spawn(spawn);
            }
        } else {
            // this should not do anything, but should still display values such as time remaining and teams
            //this.gameObject.SetActive(false);
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

    public void SpawnPlayer(int classIndex, GameObject source) {
        NetworkConnection connection = source.GetComponent<NetworkBehaviour>().connectionToClient;
        Transform startPosition = GetStartPosition();
        GameObject newPlayer = Instantiate(classPrefabs[classIndex], startPosition.position, startPosition.rotation);
        NetworkServer.Spawn(newPlayer);
        // If not a bot, move connection to a new thing
        NetworkServer.ReplacePlayerForConnection(connection, newPlayer, 0);
        GameObject.Destroy(source);
    }

}
