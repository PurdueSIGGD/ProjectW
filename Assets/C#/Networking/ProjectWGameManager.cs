using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectWGameManager : NetworkBehaviour {
    public float respawnTime = 5;
    public float timeLimit = 20 * 60 * 60; // 20 minutes
    public Transform[] startPositions;
    public GameObject playerPrefab;

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
        NetworkServer.ReplacePlayerForConnection(connection, newPlayer, 0);
        yield return new WaitForSeconds(15);
        Destroy(player.gameObject);
    }
    void Start() {
        //table = new Hashtable();
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
