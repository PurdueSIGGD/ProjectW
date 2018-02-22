using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworking : PlayerComponent {
    public Camera[] playerCameras;
    public AudioListener playerListener;

	public override void PlayerComponent_Start () {
        
        if (myBase.isLocalPlayer) {
            // If we have any spectator cameras, get rid of them
            foreach (Camera c in GameObject.FindObjectsOfType<Camera>()) {
                c.enabled = false;
            }
            foreach (AudioListener a in GameObject.FindObjectsOfType<AudioListener>()) {
                a.enabled = false;
            }
            initializeCameras(true);
        } else {
            initializeCameras(false);
        }
    }
    public void initializeCameras(bool shouldI) {
        if (playerCameras == null) {
            return;
        } else if (playerCameras.Length == 0)
        {
            return;
        }

        if (!shouldI)
        {
            foreach (Camera c in playerCameras)
            {
				c.gameObject.SetActive(false);//GameObject.Destroy();
            }
            playerCameras = null;
        }
        else
        {
            foreach (Camera c in playerCameras)
            {
                c.enabled = true;
            }
            playerListener.enabled = true;
        }
        
    }
    public override void PlayerComponent_Update() {

    }
    [ClientRpc]
    public void RpcGameOver(ProjectWGameManager.Winner winner)
    {
        Time.timeScale = 0.3f;
        if (myBase.myGUI.spectatorUIController) myBase.myGUI.spectatorUIController.GameOver(winner);
        myBase.myInput.GameOver(); 
    }
    [ClientRpc]
    public void RpcGameReset()
    {
        Time.timeScale = 1;
        myBase.myInput.Reset_GameOver();
    }
    
}
