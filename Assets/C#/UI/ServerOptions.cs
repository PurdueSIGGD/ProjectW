using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOptions : MonoBehaviour {
    public GameObject teamPrefab;
    public Transform teamList;

    public int maxTeams = 8;

    public void addTeam() {
        // Spawn new prefab into thing
        if (teamList.childCount < maxTeams) {
            GameObject.Instantiate(teamPrefab, teamList);
        }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
