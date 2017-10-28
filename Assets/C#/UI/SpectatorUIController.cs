using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpectatorUIController : MonoBehaviour {
    
    // -1 for nothing, 0 for pause, 1 for welcome, 2 for team pick, 3 for class pick
    int screenIndex = -1;
    
    public Animator pauseAnimator;
    public Animator welcomeAnimator;
    public Animator teamPickAnimator;
    public Animator classPickAnimator;

    [HideInInspector]
    public ProjectWGameManager gameManager;

    public GameObject player; // EIther spectator or actual player

    // Use this for initialization
    void Start () {
	}
	public void JoinServer() {
        SetScreenIndex(1);
        gameManager = GameObject.FindObjectOfType<ProjectWGameManager>();
    }
	public void Pause() {
        SetScreenIndex(0);
    }
    public void UnPause() {
        SetScreenIndex(-1);
        // We have to take care of whatever pause input is trying to use this
        unPause.Invoke();
    }
    public void ExitServer() {
        player.SendMessage("ExitServer");
    }
    public void ExitGame() {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
        Application.Quit();
#endif
    }

    unPauseDelegate unPause;
    public delegate void unPauseDelegate();
    public void AssignOwner(GameObject sourcePlayer, unPauseDelegate del) {
        this.unPause = del;
        player = sourcePlayer;
    }


    public void SetScreenIndex(int index) {
        screenIndex = index;
        bool pauseAnimB, welcomeAnimB, teamPickAnimB, classPickAnimB;
        pauseAnimB = welcomeAnimB = teamPickAnimB = classPickAnimB = false;
        switch (screenIndex) {
            case -1:
                unPause.Invoke();
                break;
            case 0:
                pauseAnimB = true;
                break;
            case 1:
                welcomeAnimB = true;
                break;
            case 2:
                teamPickAnimB = true;
                break;
            case 3:
                classPickAnimB = true;
                break;
            default:
                break;
        }
        pauseAnimator.SetBool("Showing", pauseAnimB);
        welcomeAnimator.SetBool("Showing", welcomeAnimB);
        teamPickAnimator.SetBool("Showing", teamPickAnimB);
        classPickAnimator.SetBool("Showing", classPickAnimB);
    }
    public void PickSpectatorTeam() {
        SetScreenIndex(-1);
    }
    public void PickTeam(int teamIndex) {
        // Save player team choice
        SetScreenIndex(3);
    }
    private int classIndex = 0;
    public void SelectClass(int classIndex) {
        this.classIndex = classIndex;
    }
    public void PickClass() {
        player.SendMessage("HandlePickingClass", classIndex);
        SetScreenIndex(-1);
    }


}
