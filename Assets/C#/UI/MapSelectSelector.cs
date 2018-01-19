using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectSelector : MonoBehaviour {
    public Dropdown mapDropdown;
    public Image mapImage;
    public MapOption[] mapOptions;
    void Start() {
        foreach (MapOption m in mapOptions) {
            mapDropdown.options.Add(new Dropdown.OptionData(m.name, m.icon));
        }
        onMapChanged(0);
        mapDropdown.value = 1; // TODO FIX THIS WORKAROUND
        mapDropdown.value = 0;
    }

	public void onMapChanged(int newVal) {
        mapImage.sprite = mapOptions[newVal].image;
    }
    [System.Serializable]
    public class MapOption {
        public Sprite image;
        public Sprite icon;
        public string name;
    }
}
