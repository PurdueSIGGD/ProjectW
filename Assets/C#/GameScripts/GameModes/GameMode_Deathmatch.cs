using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode_Deathmatch : GameMode
{
    private const string DISPLAY_NAME = "Deathmatch";

    private ProjectWGameManager manager;
    private Scoreboard scoreboard;
	private int killCount;

	public override void GameMode_Start()
    {
        manager = GameObject.FindObjectOfType<ProjectWGameManager>();
        scoreboard = GameObject.FindObjectOfType<Scoreboard>();
		killCount = getGameOption ("Max Kills");
    }
    public override string getDisplayName() {
        return DISPLAY_NAME;
    }
    public override ProjectWGameManager.Winner checkWinCondition()
    {
        if (manager != null)
        {
            // check the game's scoreboard, and see who wins
            if (manager.teams.Length == 1)
            {
                // If one team, check per individual player
                foreach (Scoreboard.ScoreboardPlayer s in scoreboard.scores)
                {
                    if (s.kills >= killCount)
                    {
                        return new ProjectWGameManager.Winner
                        {
                            winnerName = s.name,
                            winnerScore = s.kills + " Kills",
                            winnerColor = manager.teams[0].teamColor
                        };
                    }
                }
            }
            else
            {
                int[] teamScores = new int[manager.teams.Length];
                // If more than one team, check combined team scores
                foreach (Scoreboard.ScoreboardPlayer s in scoreboard.scores)
                {
                    teamScores[s.teamIndex] += s.kills;
                }
                for (int t = 0; t < teamScores.Length; t++)
                {
                    //print("team " + manager.teams[t].teamName + " score: " + teamScores[t]);
                    if (teamScores[t] >= killCount)
                    {
                        return new ProjectWGameManager.Winner
                        {
                            winnerName = manager.teams[t].teamName,
                            winnerColor = manager.teams[t].teamColor,
                            winnerScore = teamScores[t] + " Kills"
                        };
                    }
                }
            }
        }
        // Otheriwise, nobody win
        ProjectWGameManager.Winner notWon = new ProjectWGameManager.Winner
        {
            exists = false
        };
        return notWon;
    }


}
