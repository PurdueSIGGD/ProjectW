using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
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

    public Text serverIP;
    public Dropdown mapDropdown;
    public Dropdown gamemodeDropdown;

    private Text[] gamemodeOptions; // Populate when changing gamemode dropdown

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
    }

    public void StartHost() {
        print("Starting host");
    }


}
