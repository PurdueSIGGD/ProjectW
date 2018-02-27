using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerOptionsTeamItem : MonoBehaviour {
    public Button deleteButton;
    public Text nameText;
    public Dropdown colorDropdown;


    public string getTeamName() {
        return nameText.text;
    }
    public int getTeamColor() {
        return colorDropdown.value;
    }
    public void deletThis() {
        if (transform.parent.childCount > 1) {
            Destroy(this.gameObject);
        }
    }

	
}
