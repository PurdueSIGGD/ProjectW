﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerGUI : PlayerComponent {
    public bool isPaused;

    public float pauseCooldown = 1; // Cooldown, in seconds
    private float lastUse = -100; // Last time we used it, in seconds;
    private bool shouldBeLocked;

    [SyncVar]
    public int desiredPlayerClass;
    [SyncVar]
    public string desiredPlayerName;
    [SyncVar]
    public int desiredTeamIndex;

	public GameObject hudPrefab;
	private GameObject hudRoot;
	private GameHudController gameHud;
	private RectTransform healthBar;
	private RectTransform magicBar;
	[HideInInspector]
    public SpectatorUIController spectatorUIController;

    public override void PlayerComponent_Start() {
        if (isLocalPlayer && !myBase.myInput.isBot()) {
            // Don't want enemy GUIs on top of ours
			GameObject instancedHudPrefab = GameObject.Instantiate(hudPrefab, myBase.myMovement.cameraRotator);
			gameHud = instancedHudPrefab.GetComponent<GameHudController>();
			healthBar = gameHud.healthBar;
			magicBar = gameHud.magicBar;
			myBase.myStats.hitAnimator = gameHud.hitMarker;
            spectatorUIController = GameObject.FindObjectOfType<SpectatorUIController>();
			spectatorUIController.AssignOwner(this.gameObject, UnPauseGameWithoutUI, myBase.myNetworking.playerCameras[0]);
            UnPauseGame();
        } else {
			
		}
    }
    public override void PlayerComponent_Update() {
        if (isLocalPlayer) {
            // Set health and magic in GUI
            healthBar.localScale = new Vector2(myBase.myStats.health / myBase.myStats.healthMax, 1);
            magicBar.localScale = new Vector2(myBase.myStats.magic / myBase.myStats.magicMax, 1);

            // Update current abilities and their cooldowns
        }
    }

    void LateUpdate() {
        if (isLocalPlayer) {
            // Unity is buggy as fuck sometimes
            // You have to click back on the window in order to be locked again
            if (shouldBeLocked) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        

    }
    public void Death() {
		// GUI Death state
    }

    public void TogglePause() {
        if (isLocalPlayer && !myBase.myInput.isBot()) {
            if (Time.unscaledTime - lastUse > pauseCooldown) {
                Pause();
                lastUse = Time.unscaledTime;
            }
        }
    }
    private void Pause() {
        if (isLocalPlayer) {
            isPaused = !isPaused;
            if (isPaused) {
                PauseGame();
            } else {
                UnPauseGame();
            }
        }
    
        
    }
    private void PauseGame() {
        shouldBeLocked = false;
        spectatorUIController.Pause();

    }
    private void UnPauseGame() {
        shouldBeLocked = true;
        spectatorUIController.UnPause();
    }
    private void UnPauseGameWithoutUI() {
        shouldBeLocked = true;
        isPaused = false;
    }
	public void HandlePickingClass(SpectatorUIController.ClassSelectionArgs args) {
		CmdHandlePickingClass(args.classIndex, args.teamIndex, args.playerName);
    }
    [Command]
	public void CmdHandlePickingClass(int classIndex, int teamIndex, string playerName) {
        // Called by the GUI
        
        //PlayerStats myStats = this.GetComponent<PlayerStats>(); // Sometimes it doesn't initialize fast enough
        desiredPlayerClass = classIndex;
        desiredTeamIndex = teamIndex;
        desiredPlayerName = playerName;
    }
    public void ExitServer() {
        if (isServer) {
            NetworkServer.DisconnectAll();
        } else {
            Network.Disconnect();
            MasterServer.UnregisterHost();
            NetworkServer.RemoveExternalConnection(this.connectionToServer.connectionId);
        }
    }
	public void RefreshTeams() {
		CmdRefreshTeams ();
	}
	[Command]
	public void CmdRefreshTeams() {
		RpcRefreshTeams(GameObject.FindObjectOfType<ProjectWGameManager> ().teams);
	}
	[ClientRpc]
	public void RpcRefreshTeams(ProjectWGameManager.Team[] teams) {
		GameObject.FindObjectOfType<SpectatorUIController> ().teams = teams;
	}
	public void Spectate() {
		// Force our player to die and move to spectators
		CmdSpectate();
	}
	[Command]
	public void CmdSpectate() {
		this.desiredTeamIndex = -1;
		this.myBase.myStats.CmdDeath ();
		GameObject.FindObjectOfType<ProjectWGameManager>().SpawnSpectator(this.gameObject);
	}
}
