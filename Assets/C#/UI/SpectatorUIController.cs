using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpectatorUIController : MonoBehaviour {
    
	// -1 for nothing, 0 for pause, 1 for welcome, 2 for team pick, 3 for class pick
	[HideInInspector]
    public int screenIndex = -1;
    
    public Animator pauseAnimator;
    public Animator welcomeAnimator;
    public Animator teamPickAnimator;
    public Animator classPickAnimator;
    public Animator gameOverAnimator;

    [HideInInspector]
    public ProjectWGameManager gameManager;

	[HideInInspector]
    public GameObject player; // EIther spectator or actual player
    private GameObject[] playerPrefabs;
	public GameObject gridPrefab;
	public RectTransform gridParent;
	public GameObject teamPrefab;
	public RectTransform teamParent;
	public Button selectClassButton;
	[HideInInspector]
	public ProjectWGameManager.Team[] teams;
	public TrackedItemController itemTracker;

    public PrefabHolder classPrefabHolder;

    // Use this for initialization
    void Start () {

	}
	void Update() {
		selectClassButton.interactable = teamIndex != -1;
	}
   
    public void JoinServer(ProjectWGameManager.Team[] teams) {
        SetScreenIndex(1);
        gameManager = GameObject.FindObjectOfType<ProjectWGameManager>();
        RefreshTeams(teams);
		foreach (Transform child in gridParent) {
			GameObject.Destroy (child.gameObject);
		}
        playerPrefabs = classPrefabHolder.prefabs;
		for (int i = 0; i < playerPrefabs.Length; i++) {
			GameObject createdGridItem = GameObject.Instantiate (gridPrefab, gridParent);
			createdGridItem.GetComponentInChildren<Text> ().text = playerPrefabs [i].name;
			//createdGridItem.GetComponentInChildren<Image>().sprite = 
			int captured = i;
			createdGridItem.GetComponent<Button>().onClick.AddListener(() => { SelectClass(captured); });
		}
    }
    
    public void RefreshTeams(ProjectWGameManager.Team[] teams) {
        print("refreshing teams");
		foreach (Transform child in teamParent) {
			GameObject.Destroy (child.gameObject);
		}
		this.teams = teams;
		for (int i = 0; i < teams.Length; i++) {
			GameObject createdGridItem = GameObject.Instantiate (teamPrefab, teamParent);
            //print("team stuff " + teams[i].teamColor);
            createdGridItem.GetComponentInChildren<Text>().text = teams [i].teamName;
            createdGridItem.GetComponentInChildren<Text>().color = teams[i].teamColor;
            // TODO createdGridItem.GetComponentInChildren<Image>().sprite = teams[i].teamSprite;
            int captured = i;
			createdGridItem.GetComponentInChildren<Button>().onClick.AddListener(() => { PickTeam(captured); });
		}
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
    public void AssignOwner(GameObject sourcePlayer, unPauseDelegate del, Camera myCamera) {
        this.unPause = del;
        player = sourcePlayer;
        itemTracker.UpdateCamera(myCamera);
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
		this.teamIndex = -1;
        SetScreenIndex(-1);
		player.SendMessage ("Spectate");
    }
    public void PickTeam(int teamIndex) {
        // Save player team choice
        SetScreenIndex(3);
		this.teamIndex = teamIndex;
    }
    private int classIndex = 0;
	private int teamIndex = -1;
	public Text className;
	public Text classDescription;
	public Text playerName;
	public struct ClassSelectionArgs {
		public int classIndex;
		public int teamIndex;
		public string playerName;
	}
    public void SelectClass(int classIndex) {
		//print (classIndex);
        this.classIndex = classIndex;
		className.text = playerPrefabs [classIndex].name;
		classDescription.text = "";
		//classDescription.text = playerPrefabs [classIndex].description;

    }
    public void PickClass() {
		ClassSelectionArgs args = new ClassSelectionArgs ();
		args.classIndex = classIndex;
		args.teamIndex = teamIndex;
		args.playerName = playerName.text;
		player.SendMessage("HandlePickingClass", args);
        SetScreenIndex(-1);
    }
    public void GameOver(ProjectWGameManager.Winner winner)
    {
        gameOverAnimator.SetBool("Showing", true);
        pauseAnimator.SetBool("Showing", false);
        welcomeAnimator.SetBool("Showing", false);
        teamPickAnimator.SetBool("Showing", false);
        classPickAnimator.SetBool("Showing", false);
        // TODO set game over winner and score

    }


}
