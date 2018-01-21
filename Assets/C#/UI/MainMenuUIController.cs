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

    // 0 for main page, 1 for join server, 2 for map select, 3 for map settings
    [HideInInspector]
    public int screenIndex = 0;

    public Animator mainAnimator;
    public Animator joinGameAnimator;
    public Animator newGameAnimator;
    public Animator serverOptions;

	public Animator invalidOptionAnimator;

    public Text serverIP;
    public Dropdown mapDropdown;
    public Dropdown gamemodeDropdown;
    public Transform gameModeOptionsParent;

    public Transform teamList;

	public ProjectWNetworkManager networkManager;

	public GameObject teamColors;

	public string[] mapList;


    void Start() {
        SetScreenIndex(0);
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
        PlayerPrefs.SetString(PlayerPrefStrings.IP_TO_CONNECT, ip);
        print("Joining server");
        Network.Connect(ip, 7777);
        // TODO verify this works
    }

    public void StartHost() {
        print("Starting host");

		ServerOptionsTeamItem[] teamItems = teamList.GetComponentsInChildren<ServerOptionsTeamItem>();
	
        int mapSelect = mapDropdown.value;
        int gameModeSelect = gamemodeDropdown.value;

		networkManager.teamItems = new ProjectWGameManager.Team[teamItems.Length];
		int index = 0;
		foreach (ServerOptionsTeamItem teamItem in teamItems) {
			networkManager.teamItems[index] = new ProjectWGameManager.Team ();
			networkManager.teamItems[index].teamIndex = index;
			networkManager.teamItems[index].teamColor = teamColors.GetComponent<ColorHolder>().colors [teamItem.colorDropdown.value];
			networkManager.teamItems[index].teamName = teamItem.nameText.text;
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
				networkManager.gamemodeOptions [index].optionName = option.displayText.text;
				networkManager.gamemodeOptions [index].value = parsedValue;
			} else {
				// display warning
				invalidOptionAnimator.SetTrigger("Error");
				return;
			}
			// TODO save to playerprefs
			index++;
		}


		networkManager.StartHost();
		//SceneManager.LoadScene (mapList [mapSelect], LoadSceneMode.Single);

    }


}
