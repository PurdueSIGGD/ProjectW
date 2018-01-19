using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeSelector : MonoBehaviour {
	public Dropdown gamemodeDropdown;
	public PrefabHolder gameModePrefabHolder;
	public GameObject optionTemplate;

	public void Start() {
        foreach (GameObject g in gameModePrefabHolder.prefabs) {
            gamemodeDropdown.options.Add(new Dropdown.OptionData(g.GetComponent<GameMode>().getDisplayName()));
        }
		OnDropdownChange (0);
	}

	public void OnDropdownChange(int newVal) {
		// Delete all options, populate from gameobject
		//print("new val: " + newVal);
        foreach (GameModeUIOption g in transform.GetComponentsInChildren<GameModeUIOption>()) {
            GameObject.Destroy(g.gameObject);
        }
        // TODO for each item, spawn a template
        foreach (GameMode.GameOption o in gameModePrefabHolder.prefabs[newVal].GetComponent<GameMode>().gameOptions) {
            GameModeUIOption option = GameObject.Instantiate(optionTemplate, transform).GetComponent<GameModeUIOption>();
            option.displayText.text = o.optionName;
            option.editText.text = o.value + "";
        }
        
	}

	public GameMode.GameOption[] getGameOptions() {
		GameMode.GameOption[] gameOptions = new GameMode.GameOption[0];
		return gameOptions;
	}

    public void ChangeDropdown() {
        OnDropdownChange(gamemodeDropdown.value);
    }
}
