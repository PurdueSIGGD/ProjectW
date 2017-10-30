using System.Collections;
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
    public int classIndex;
	[SyncVar]
	public int teamIndex;
	[SyncVar]
	public string playerName;

    public GameObject rootGUI;
    public RectTransform healthBar;
    public RectTransform magicBar;

    public SpectatorUIController spectatorUIController;

    public override void PlayerComponent_Start() {
        if (isLocalPlayer && !myBase.myInput.isBot()) {
            // Don't want enemy GUIs on top of ours
            rootGUI.SetActive(true);
            spectatorUIController = GameObject.FindObjectOfType<SpectatorUIController>();
            spectatorUIController.AssignOwner(this.gameObject, UnPauseGameWithoutUI);
            UnPauseGame();
        } else {
            rootGUI.SetActive(false);
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
        rootGUI.SetActive(false);
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
		this.classIndex = classIndex;
		this.teamIndex = teamIndex;
		this.playerName = playerName;
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
		this.myBase.myStats.teamIndex = -1;
		this.myBase.myStats.CmdDeath ();
		GameObject.FindObjectOfType<ProjectWGameManager>().SpawnSpectator(this.gameObject);
	}
}
