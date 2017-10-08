using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectWNetworkManager : NetworkManager {
    public GameObject lobbyCamera;
    public GameObject[] spawnablePrefabs;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        Transform start = GetStartPosition();
        GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        if (lobbyCamera != null) {
            lobbyCamera.SetActive(false);
        }
    }

    // called when a client connects 
    public override void OnServerConnect(NetworkConnection conn) {

    }

    // called when a client disconnects
    public override void OnServerDisconnect(NetworkConnection conn) {
        NetworkServer.DestroyPlayersForConnection(conn);
    }

    // called when a client is ready
    public override void OnServerReady(NetworkConnection conn) {
        NetworkServer.SetClientReady(conn);
    }
    
    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player) {
        base.OnServerRemovePlayer(conn, player);
    }
    
    // called when a network error occurs
    public override void OnServerError(NetworkConnection conn, int errorCode) {

    }
}
