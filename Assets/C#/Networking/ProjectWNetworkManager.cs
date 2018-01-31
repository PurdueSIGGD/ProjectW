﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ProjectWNetworkManager : NetworkManager {
    public GameObject lobbyCamera;

	// START GAME SERVER SETTINGS
	public ProjectWGameManager.Team[] teamItems;
	public int mapSelect;
	public int gameModeSelect;
	public GameMode.GameOption[] gamemodeOptions;
	// END GAME SERVER SETTINGS




    public void Start() {
        if (lobbyCamera != null) {
            lobbyCamera.SetActive(true);
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        Transform start = GetStartPosition();
        GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        if (lobbyCamera != null) {
            lobbyCamera.SetActive(false);
        }
		player.SendMessage("JoinServer");
    }

	public override void OnClientConnect(NetworkConnection conn) {
		/*if (string.IsNullOrEmpty (this.onlineScene) || this.onlineScene == this.offlineScene) {
			ClientScene.Ready (conn);
			ClientScene.AddPlayer (conn, 0, new StringMessage("Welcome I guess"));

		}*/
		//ClientScene.AddPlayer (conn, 0, new StringMessage("Welcome I guess"));
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


    public IEnumerator TrackPing(Scoreboard scoreBoard, GameObject player, int playerId)
    {
        yield return new WaitForSeconds(5);
        if (playerId > 0)
        {

            string ping = "TODO"; //TODO
            scoreBoard.UpdatePing(playerId, ping);
            // repeat
        }
        else
        {
            string ping = "BOT";
            scoreBoard.UpdatePing(playerId, ping);
            //Dont repeat, this is a bot
        }
    }

	public void ResetCursor() {
		// Make sure we can still click and see our cursor
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}


}
