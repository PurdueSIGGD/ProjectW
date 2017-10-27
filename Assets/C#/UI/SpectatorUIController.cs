using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectatorUIController : MonoBehaviour {
    public GameObject pauseMenuPrefab;
    [HideInInspector]
    public GameObject pauseMenu;
    private unPauseDelegate del;

	// Use this for initialization
	void Start () {
        pauseMenu = GameObject.Instantiate(pauseMenuPrefab, transform);
	}
	
	public void Pause() {
        pauseMenu.SetActive(true);
    }
    public void UnPause() {
        pauseMenu.SetActive(false);
    }
    public delegate void unPauseDelegate();
    public void SetUnpause(unPauseDelegate del) {
        this.del = del;
        pauseMenu.GetComponentsInChildren<Button>()[0].onClick.AddListener(delAction);
    }
    public void delAction() {
        del.Invoke();
    }
}
