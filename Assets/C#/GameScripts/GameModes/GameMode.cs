using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode : MonoBehaviour {
	public GameOption[] gameOptions;
    // Base class for gamemode types
    public abstract ProjectWGameManager.Winner checkWinCondition();
    public abstract string getDisplayName();

	// Every gamemode must define the options and settings for the gamemode

	public abstract void GameMode_Start () ;
	public void Start() {
		GameMode_Start ();
	}
	public int getGameOption(string optionName) {
		foreach (GameOption option in gameOptions) {
			if (option.optionName == optionName)
				return option.value;
		}
		return -1;
	}
	[System.Serializable]
	public class GameOption {
		public GameOption(string optionName, int defaultOptionValue) {
			this.optionName = optionName;
			this.value = defaultOptionValue;
		}
		public string optionName;
		public int value;
	}
}
