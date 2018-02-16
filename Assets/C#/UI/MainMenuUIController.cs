using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuUIController : MonoBehaviour {
	/**
	 * 
	 * TODO:
	 * 
	 * Server welcome message
	 * Server max players
	 * Server bots & difficulty (per team)
	*/



    // 0 for main page, 1 for join server, 2 for map select, 3 for map settings
    [HideInInspector]
    public int screenIndex = 0;

    public Animator mainAnimator;
    public Animator joinGameAnimator;
    public Animator newGameAnimator;
    public Animator serverOptions;

	public Animator invalidOptionAnimator;
	public Text invalidAnimatorText;

    public Text serverIP;
	public InputField serverIPInput;
    public Dropdown mapDropdown;
    public Dropdown gamemodeDropdown;
    public Transform gameModeOptionsParent;

    public Transform teamList;

	public GameObject networkManagerPrefab;
	private ProjectWNetworkManager networkManager;

	public GameObject teamColors;
	public SceneHolder sceneHolder;


	private const string ERROR_NONUMBER = "PLEASE ENTER A NUMBER FOR GAMEMODE OPTIONS";
	private const string ERROR_UNIQUETEAMS = "PLEASE ENSURE TEAMS ARE UNIQUE";


    void Start() {
		if (PlayerPrefs.HasKey (PlayerPrefStrings.SAVED_IP_TO_CONNECT)) {
			serverIPInput.text = PlayerPrefs.GetString (PlayerPrefStrings.SAVED_IP_TO_CONNECT);
		} else {
			print ("No player pref server found");
		}
		SetScreenIndex(0);
		ResetCursor ();
		if ((networkManager = GameObject.FindObjectOfType<ProjectWNetworkManager> ())) {
			NetworkManager.Shutdown ();
		}
		networkManager = GameObject.Instantiate (networkManagerPrefab).GetComponent<ProjectWNetworkManager> ();
		// Dropdown workaround refreshes
		mapDropdown.value = 1;
		gamemodeDropdown.value = 1;
		mapDropdown.value = 0;
		gamemodeDropdown.value = 0;
    }
    public void ExitGame() {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
        Application.Quit();
#endif
    }


    public void SetScreenIndex(int index) {
        screenIndex = index;
        bool mainAnimB, joinAnimB, newGameAnimB, serverOptionsAnimB;
        mainAnimB = joinAnimB = newGameAnimB = serverOptionsAnimB = false;
        switch (screenIndex) {
            case -1:
                break;
            case 0:
                mainAnimB = true;
                break;
            case 1:
                joinAnimB = true;
                break;
            case 2:
                newGameAnimB = true;
                break;
            case 3:
                serverOptionsAnimB = true;
                break;
            default:
                break;
        }
        mainAnimator.SetBool("Showing", mainAnimB);
        joinGameAnimator.SetBool("Showing", joinAnimB);
        newGameAnimator.SetBool("Showing", newGameAnimB);
        serverOptions.SetBool("Showing", serverOptionsAnimB);
    }
    
    public void JoinServer() {
        string ip = serverIP.text;
        PlayerPrefs.SetString(PlayerPrefStrings.SAVED_IP_TO_CONNECT, ip);
        print("Joining server");
		networkManager.networkAddress = ip;
		networkManager.networkPort = 4718;
		networkManager.StartClient ();
        // TODO verify this works
    }

    public void StartHost() {
        print("Starting host");
		networkManager.gameObject.SetActive (true);

		ServerOptionsTeamItem[] teamItems = teamList.GetComponentsInChildren<ServerOptionsTeamItem>();
	
		string mapSelect = sceneHolder.scenes[mapDropdown.value].name;
        int gameModeSelect = gamemodeDropdown.value;

		networkManager.teamItems = new ProjectWGameManager.Team[teamItems.Length];
		int index = 0;
		foreach (ServerOptionsTeamItem teamItem in teamItems) {
			networkManager.teamItems[index] = new ProjectWGameManager.Team ();
			networkManager.teamItems[index].teamIndex = index;
			networkManager.teamItems[index].teamColor = teamColors.GetComponent<ColorHolder>().colors [teamItem.colorDropdown.value];
			networkManager.teamItems[index].teamName = teamItem.nameText.text;
            networkManager.teamItems[index].teamSprite = teamItem.colorDropdown.value;
			for (int i = index - 1; i >= 0; i--) {
				// Go back, ensure uniqueness
				if (networkManager.teamItems [i].teamColor == networkManager.teamItems [index].teamColor ||
					networkManager.teamItems [i].teamName == networkManager.teamItems [index].teamName) {
					SetError (ERROR_UNIQUETEAMS);
					return;
				}
			}
			index++;
		}
		networkManager.mapSelect = mapSelect;
		networkManager.gameModeSelect = gameModeSelect;

		GameModeUIOption[] options = gameModeOptionsParent.GetComponentsInChildren<GameModeUIOption> ();

		networkManager.gamemodeOptions = new GameMode.GameOption[options.Length];
		index = 0;
		foreach (GameModeUIOption option in options) {
			string optionValue = option.editText.text;
			int parsedValue;
			if (int.TryParse (optionValue, out parsedValue)) {
				networkManager.gamemodeOptions [index] = new GameMode.GameOption (option.displayText.text, parsedValue);
			} else {
				// display warning
				SetError(ERROR_NONUMBER);
				return;
			}
			index++;
		}

		networkManager.onlineScene = mapSelect;
		networkManager.StartHost();

    }
	private void SetError(string errorMessage) {
		invalidAnimatorText.text = errorMessage;
		invalidOptionAnimator.SetTrigger("Error");
	}
	public void ResetCursor() {
		// Make sure we can still click and see our cursor
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}
