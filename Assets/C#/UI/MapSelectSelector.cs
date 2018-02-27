using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectSelector : MonoBehaviour {
    public Dropdown mapDropdown;
    public Image mapImage;
	public SceneHolder sceneHolder;
    void Start() {
		foreach (SceneHolder.Scene s in sceneHolder.scenes) {
            mapDropdown.options.Add(new Dropdown.OptionData(s.name, s.icon));
        }
        onMapChanged(0);
        mapDropdown.value = 1; // TODO FIX THIS WORKAROUND WITH REFRESHING
        mapDropdown.value = 0;
    }

	public void onMapChanged(int newVal) {
		mapImage.sprite = sceneHolder.scenes[newVal].image;
    }
}
