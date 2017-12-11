using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spectator : NetworkBehaviour {
    public ParticleSystem myParticles;
    public GameObject myCamera;
    public float moveSpeed = 1;
    private Rigidbody myRigid;
    private SpectatorUIController uiController;

    // UI Stuff
    public bool isPaused;
    public float pauseCooldown = 1; // Cooldown, in seconds
    private float lastUse = -100; // Last time we used it, in seconds;
    private bool shouldBeLocked;
    private bool hasJoined;

    // Use this for initialization
    void Start () {
        myRigid = this.GetComponent<Rigidbody>();
        uiController = GameObject.FindObjectOfType<SpectatorUIController>();
       
      
        if (isLocalPlayer)
        {
            myParticles.Pause();
			uiController.AssignOwner(this.gameObject, UnPauseGameWithoutUI, myCamera.GetComponent<Camera>());
            
			shouldBeLocked = uiController.screenIndex == -1;
			isPaused = !shouldBeLocked;

        } else {
            GameObject.Destroy(myCamera);
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

    // Update is called once per frame
    void FixedUpdate () {
        if (isLocalPlayer) {
            bool pause = Input.GetAxis("Pause") > 0;
            if (pause) {
                TogglePause();
            }

            if (isPaused) {

            } else {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                float up = Input.GetAxis("Jump");
                float down = Input.GetAxis("Crouch");


                float newX = transform.rotation.eulerAngles.x + (mouseY * Time.deltaTime);
                //We do some fancy math to ensure 0 < newX < 360, nothing more
                newX = (newX + 360) % 360;
                //Ensure it doesn't go past our top or low bounds
                if ((newX > 0 && newX < 90) || (newX < 360 && newX > 270)) {
                    // Camera rotation
                    myRigid.MoveRotation(Quaternion.Euler(newX, transform.eulerAngles.y + (mouseX * Time.deltaTime), transform.eulerAngles.z));
                } else {
                    // We don't want you to look all the way behind you, that's weird
                    myRigid.MoveRotation(Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + mouseX * Time.deltaTime, transform.eulerAngles.z));
                }


                Vector3 dirVector = new Vector3(horizontal, up - down, vertical);
                float distanceMultiplier = Time.deltaTime * moveSpeed;
                myRigid.MovePosition(transform.position + transform.TransformDirection(dirVector) * distanceMultiplier);

                
            }
           

        }
       
    }
	public void EnableCursor() {
		shouldBeLocked = false;
	}
    public void TogglePause() {
        if (isLocalPlayer) {
            if (Time.unscaledTime - lastUse > pauseCooldown) {
                Pause();
                lastUse = Time.unscaledTime;
            } else {

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
        uiController.Pause();

    }
    private void UnPauseGame() {
        shouldBeLocked = true;
        uiController.UnPause();
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
		GameObject.FindObjectOfType<ProjectWGameManager>().SpawnPlayer(classIndex, teamIndex, playerName, this.gameObject);
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
	
	public void JoinServer() {
        // Called by the network manager when we join a server, only happens on the server side
        RpcJoinServer(GameObject.FindObjectOfType<ProjectWGameManager>().teams);
	}
    [ClientRpc]
    public void RpcJoinServer(ProjectWGameManager.Team[] teams)
    {
        if (isLocalPlayer)
        {
            shouldBeLocked = false; // We enable the mouse since the UI pops up
            isPaused = true;
            GameObject.FindObjectOfType<SpectatorUIController>().JoinServer(teams);
        }
    }
    public void Spectate() {
        // Nothing
    }
    [ClientRpc]
    public void RpcGameOver(ProjectWGameManager.Winner winner)
    {
        Time.timeScale = 0.3f;
        uiController.GameOver(winner);
    }
    [ClientRpc]
    public void RpcGameReset()
    {
        Time.timeScale = 1;
    }
}
