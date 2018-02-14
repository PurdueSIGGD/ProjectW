using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Scoreboard : NetworkBehaviour {
    public RectTransform parentGroup; // Scoreboard holder for each item
    public GameObject scoreboardItemPrefab; // Prefab for the scoreboard item we use
    public Sprite[] classSpritePrefabHolder; // Has all sprites for each character 
    
    public SyncListScoreboardPlayer scores = new SyncListScoreboardPlayer();

    public class SyncListScoreboardPlayer : SyncListStruct<ScoreboardPlayer> {
        public ScoreboardPlayer Find(int id)
        {
            foreach (ScoreboardPlayer p in this)
            {
                if (p.id == id) return p;
            }
            return new ScoreboardPlayer
            {
                found = false
            };
        }
        public class ScoreboardPlayerComparer : IComparer
        {
            int IComparer.Compare(object a, object b)
            {
                ScoreboardPlayer sa = (ScoreboardPlayer)a;
                ScoreboardPlayer sb = (ScoreboardPlayer)b;
                if (sa.kills > sb.kills)
                {
                    return -1;
                } else if (sa.kills < sb.kills)
                {
                    return 1;
                }
                else
                {
                    // Equals, use assists 
                    if (sa.assists > sb.assists)
                    {
                        return -1;
                    } else if (sa.assists < sb.assists)
                    {
                        return 1;
                    }
                    else
                    {
                        // Equals, use min deaths 
                        if (sa.deaths < sb.deaths)
                        {
                            return -1;
                        } else if (sa.deaths > sb.deaths)
                        {
                            return 1;
                        }
                        else
                        {
                            // Well that's a draw 
                            return 0;
                        }
                    }
                }
            }
        }
    }

    [System.Serializable]
    public struct ScoreboardPlayer 
    {
        public string name;
        public int teamIndex;
        public int classId;
        public int kills;
        public int assists;
        public int deaths;
        public String ping;
        public int id; // Player Ids from playerInput
        public bool found; // Used for finding and whatnot
    }
    public void UpdatePing(int id, String newPing)
    {
        //print("updating scores " + id + " " + diffKills);
        int foundPlayerIndex = FindPlayerIndex(id);
        if (foundPlayerIndex != -1)
        {

            ScoreboardPlayer foundPlayer = scores.GetItem(foundPlayerIndex);
            foundPlayer.ping = newPing;
            scores[foundPlayerIndex] = foundPlayer;

        }
    }
    
    public void UpdateScore(int id, int diffKills, int diffAssists, int diffDeaths)
    {
        //print("updating scores " + id + " " + diffKills);
        // TODO update ping on a timer, not just through here
        string ping = getPing(id);

        int foundPlayerIndex = FindPlayerIndex(id);
		if (foundPlayerIndex != -1) {

			ScoreboardPlayer foundPlayer = scores.GetItem (foundPlayerIndex);
			foundPlayer.kills += diffKills;
			foundPlayer.assists += diffAssists;
			foundPlayer.deaths += diffDeaths;
			foundPlayer.ping = ping;
			scores [foundPlayerIndex] = foundPlayer;

		} 
    }
	public void RemovePlayer(int id) {
		// Called when player disconnects under any circumstances
		ScoreboardPlayer findResult = scores.Find(id);
		print ("removing player: " + findResult.name);
		bool removeResult = scores.Remove(findResult);
		if (removeResult) {
			RpcRefreshScoreboard();
		} else {
			print ("Could not find player to disconnect, sorry");
		}

	}

	[ClientRpc]
	public void RpcRefreshScoreboard() {
		RefreshScoreboard();
	}

	// Use this for initialization
	void Start () {
        scores.Callback = OnScoreboardUpdated;
        RefreshScoreboard();
    }

    private void OnScoreboardUpdated(SyncListScoreboardPlayer.Operation op, int index)
    {
        RefreshScoreboard();
        
    }
    private void RefreshScoreboard()
    {



        ScoreboardPlayer[] tmp = scores.ToArray();
        Array.Sort(tmp, (IComparer)new SyncListScoreboardPlayer.ScoreboardPlayerComparer());
        // Sort, then put up

        //foreach (ScoreboardPlayer scoreboardPlayer in tmp)
        for (int i = 0; i < scores.Count; i++)
        {
			
            ScoreboardPlayer scoreboardPlayer = tmp[i];
            Transform foundItem;
            if ((foundItem = parentGroup.Find(scoreboardPlayer.id.ToString())) == null)
            {
                // Make a new UI item
                foundItem = GameObject.Instantiate(scoreboardItemPrefab, parentGroup).transform;
            }
            // Update 
            foundItem.SetSiblingIndex(i);
            ScoreboardItem scoreboardItem = foundItem.GetComponent<ScoreboardItem>();
            scoreboardItem.gameObject.name = scoreboardPlayer.id.ToString();
            scoreboardItem.nameText.text = scoreboardPlayer.name;
            scoreboardItem.score.text = scoreboardPlayer.kills.ToString();
            scoreboardItem.assists.text = scoreboardPlayer.assists.ToString();
            scoreboardItem.deaths.text = scoreboardPlayer.deaths.ToString();
            scoreboardItem.ping.text = scoreboardPlayer.ping;
            scoreboardItem.id = scoreboardPlayer.id;
            scoreboardItem.classIcon.sprite = classSpritePrefabHolder[scoreboardPlayer.classId];

            Color teamColor = Color.white;
			// All players are added, including spectators, who are not colored 
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                //print(player.name + " " + player.GetComponent<NetworkBehaviour>().isLocalPlayer);
                BasePlayer basePlayer = player.GetComponent<BasePlayer>();
                int playerId = basePlayer.GetComponent<PlayerInput>().GetPlayerId();
                if (playerId == scoreboardPlayer.id)
                {
					scoreboardItem.referredItem = player;
                    teamColor = basePlayer.GetComponent<PlayerStats>().teamColor;
                    if (player.GetComponent<NetworkBehaviour>().isLocalPlayer)
                    {
                        // Is the local player
                        scoreboardItem.nameText.fontStyle = FontStyle.Bold;
                        // TODO make it a different color, better
                        scoreboardItem.nameText.color = Color.black;
                    }
                }

            }
            foreach (Image image in scoreboardItem.teamImages)
            {
                image.color = teamColor;
            }
        }
		foreach (ScoreboardItem scoreboardItem in parentGroup.GetComponentsInChildren<ScoreboardItem>()) {
			if (!scores.Find (scoreboardItem.id).found) {
				// Delete if not found
				GameObject.Destroy(scoreboardItem.gameObject);
			}
		}

		//print ("Score synclist size: " + scores.Count);
        
    }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < Network.connections.Length; i++)
        {
            //print("Player " + i + " " + Network.GetAveragePing(Network.connections[i]));
        }
        if (!isServer) return; // All scoreboard processing is handled server side
        
        foreach (PlayerInput p in GameObject.FindObjectsOfType<PlayerInput>())
        {
            if (p.GetPlayerId() == 0)
            {
                // Unassigned, we can't do anything here
                continue;
            }
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
                print("Adding player with id " + p.GetPlayerId());
                scores.Add(new ScoreboardPlayer
                {
                    name = ps.gameObject.name,
                    teamIndex = ps.teamIndex,
                    classId = ps.classIndex,
                    kills = 0,
                    deaths = 0,
                    assists = 0,
                    id = p.GetPlayerId(),
                    ping = getPing(p.GetPlayerId()),
					found = true
                });
            }
        }
		// If any have not been checked, then kill and try again
		// TODO OMG FUCKING KILL ME
		bool refresh = false;
		foreach (ScoreboardItem items in parentGroup.GetComponentsInChildren<ScoreboardItem>()) {
			if (items.referredItem == null) {
				scores.Remove (scores.Find(items.id));
				refresh = true;
			}
		}
		if (refresh) {
			
			RpcRefreshScoreboard ();
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
    string getPing(int id)
    {
        string ping = "BOT";
        if (id == 1) ping = "0";
        else if (id > 0)
        {
            print(NetworkServer.connections.Count + " ");
            ping = "0";
            //ping = (NetworkServer.connections[id - 2]).00;
        }
        return ping;
    }
}
