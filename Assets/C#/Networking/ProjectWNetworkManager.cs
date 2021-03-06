﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ProjectWNetworkManager : NetworkManager {
    public GameObject lobbyCamera;
	public GameObject playerPrefabs;

	// START GAME SERVER SETTINGS
	public ProjectWGameManager.Team[] teamItems;
	public ProjectWGameManager.Bot[] botItems;
	public string mapSelect;
	public int gameModeSelect;
	public GameMode.GameOption[] gamemodeOptions;
	public SceneHolder scenesToLoad;
	// END GAME SERVER SETTINGS
	private Scoreboard scoreBoard;




    public void Start() {
        if (lobbyCamera != null) {
            lobbyCamera.SetActive(true);
        }
		foreach (GameObject playerPrefab in playerPrefabs.GetComponent<PrefabHolder>().prefabs) {
			ClientScene.RegisterPrefab (playerPrefab);
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
	
    // called when a client disconnects
    public override void OnServerDisconnect(NetworkConnection conn) {
        // We don't destroy these guys... we just disable them cause it crashes the game 
        foreach (PlayerController p in conn.playerControllers) {
            p.gameObject.SetActive(false);
        }
        if (scoreBoard == null) {
            scoreBoard = GameObject.FindObjectOfType<ProjectWGameManager>().scoreBoard;
        }
        scoreBoard.RpcRefreshScoreboard();
        //NetworkServer.DestroyPlayersForConnection(conn);
    }
    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player) {
        // We don't destroy these guys... we just disable them cause it crashes the game 
        player.gameObject.SetActive(false);
        //base.OnServerRemovePlayer(conn, player);
    }
    /*
    // called when a client is ready
    public override void OnServerReady(NetworkConnection conn) {
        NetworkServer.SetClientReady(conn);
    }
    
    // called when a network error occurs
	public override void OnServerError(NetworkConnection conn, int errorCode) {
		//RemovePlayerScoreboard (conn.connectionId);
    }

	*/



    /*public void RemovePlayerScoreboard(int connectionId) {
		print ("Client disconnected: " + connectionId);
		if (scoreBoard == null) {
			scoreBoard = GameObject.FindObjectOfType<ProjectWGameManager> ().scoreBoard;
		}
		scoreBoard.RemovePlayer (connectionId);
	}*/


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
