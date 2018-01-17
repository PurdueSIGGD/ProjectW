using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeSelector : MonoBehaviour {
	public Dropdown gamemodeDropdown;
	public PrefabHolder gameModePrefabHolder;
	public GameObject optionTemplate;

	public void Start() {
		// Add dropdown options from it

		OnDropdownChange (0);
	}

	public void OnDropdownChange(int newVal) {
		// Delete all options, populate from gameobject
		print("new val: " + newVal);
	}

	public GameMode.GameOption[] getGameOptions() {
		GameMode.GameOption[] gameOptions = new GameMode.GameOption[0];
		return gameOptions;
	}
}
