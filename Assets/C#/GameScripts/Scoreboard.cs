using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Scoreboard : NetworkBehaviour {
    [SyncVar]
    public SyncListScoreboardPlayer scores = new SyncListScoreboardPlayer();
    int scoreIndex = 0;

    public class SyncListScoreboardPlayer : SyncListStruct<ScoreboardPlayer> { }

    [System.Serializable]
    public struct ScoreboardPlayer 
    {
        public string name;
        public int teamIndex;
        public int kills;
        public int assists;
        public int deaths;
        public int id; // Player Ids from playerInput
        public bool found; // Used for finding and whatnot
    }
    public void UpdateScore(int id, int diffKills, int diffAssists, int diffDeaths)
    {
        //print("updating scores " + id + " " + diffKills);
        int foundPlayerIndex = FindPlayerIndex(id);
        if (foundPlayerIndex != -1)
        {

            ScoreboardPlayer foundPlayer = scores.GetItem(foundPlayerIndex);
            foundPlayer.kills += diffKills;
            foundPlayer.assists += diffAssists;
            foundPlayer.deaths += diffDeaths;
            scores[foundPlayerIndex] = foundPlayer;

        }
       
    }

	// Use this for initialization
	void Start () {
        scores.Callback = OnScoreboardUpdated;
    }

    private void OnScoreboardUpdated(SyncListScoreboardPlayer.Operation op, int index)
    {
        // TODO update scoreboard UI here
    }

    // Update is called once per frame
    void Update () {
        if (!isServer) return; // All scoreboard processing is handled server side
        
        foreach (PlayerInput p in GameObject.FindObjectsOfType<PlayerInput>())
        {
            int foundIndex = FindPlayerIndex(p.GetPlayerId());
            PlayerStats ps = p.GetComponent<PlayerStats>();
            if (foundIndex != -1)
            {
                ScoreboardPlayer foundPlayer = scores.GetItem(foundIndex);
                foundPlayer.name = ps.gameObject.name;
                foundPlayer.teamIndex = ps.teamIndex;
                scores[foundIndex] = foundPlayer;

            } else
            {
                //print("Adding player with id " + p.GetPlayerId());
                scores.Add(new ScoreboardPlayer
                {
                    name = ps.gameObject.name,
                    teamIndex = ps.teamIndex,
                    kills = 0,
                    deaths = 0,
                    assists = 0,
                    id = p.GetPlayerId(),
                    found = true
                });
            }
        }
	}
    ScoreboardPlayer FindPlayer(int id)
    {
        int i = 0;
        foreach (ScoreboardPlayer sbp in scores)
        {
            //print("score item " + sbp.name);
            if (sbp.id == id)
            {
                return sbp;
            }
            i++;
        }
        //print("not found");
        return new ScoreboardPlayer
        {
            found = false
        };
        
    }
    int FindPlayerIndex(int id)
    {
        int i = 0;
        foreach (ScoreboardPlayer sbp in scores)
        {
            //print("score item " + sbp.name);
            if (sbp.id == id)
            {
                return i;
            }
            i++;
        }
        //print("not found");
        return -1;
    }

}
