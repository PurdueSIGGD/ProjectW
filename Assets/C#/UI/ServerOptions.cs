using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerOptions : MonoBehaviour {
    public GameObject teamPrefab;
    public Transform teamList;

    public int maxTeams = 8;

    public void addTeam() {
        // Spawn new prefab into thing
        if (teamList.childCount < maxTeams) {
			ServerOptionsTeamItem spawnedItem = GameObject.Instantiate(teamPrefab, teamList).GetComponent<ServerOptionsTeamItem>();
			spawnedItem.nameText.GetComponent<InputField> ().text = "Team " + teamList.childCount;
			spawnedItem.colorDropdown.value = 0;
			spawnedItem.colorDropdown.value = Random.Range (0, maxTeams - 1);
        }
    }


	// Use this for initialization
	void Start () {
		addTeam ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
