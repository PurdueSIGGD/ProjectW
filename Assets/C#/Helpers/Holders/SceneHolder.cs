using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHolder : MonoBehaviour {
	/* Used to keep track of each scene in our game */
	public Scene[] scenes;
	[System.Serializable]
	public class Scene {
		public Sprite image;
		public Sprite icon;
		public string name;
	}
}
