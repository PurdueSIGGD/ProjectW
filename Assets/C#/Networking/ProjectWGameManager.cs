using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectWGameManager : NetworkBehaviour {
    public float respawnTime = 5;
    public float timeLimit = 20 * 60 * 60; // 20 minutes
    public Transform[] startPositions;
    public GameObject playerPrefab;
    public int botCount = 0;

    //private Hashtable table;
    
    public void AddDeath(GameObject player, string playerid) {
        //print("Adding death as player: " + playerid);
        //table.Add(playerid, true);
        StartCoroutine(cRespawnPlayer(player.GetComponent<PlayerStats>()));
    }
    public IEnumerator cRespawnPlayer(PlayerStats player) {
        yield return new WaitForSeconds(respawnTime);
        NetworkConnection connection = player.connectionToClient;
        Transform startPosition = GetStartPosition();
        GameObject newPlayer = Instantiate(playerPrefab, startPosition.position, startPosition.rotation);
        // If not a bot, move connection to a new thing
        if (player.playerControllerId != -1) {
            NetworkServer.ReplacePlayerForConnection(connection, newPlayer, 0);
        }
        yield return new WaitForSeconds(15);
        Destroy(player.gameObject);
    }
    void Start() {
        //table = new Hashtable();
        if (isServer) {
            for (int i = 0; i < botCount; i++) {
                Transform startPosition = GetStartPosition();
                GameObject spawn = Instantiate(playerPrefab, startPosition.position, startPosition.rotation);
                spawn.SendMessage("setBot");
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
