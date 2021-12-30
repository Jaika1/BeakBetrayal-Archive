using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public TMP_Text[] LeaderboardTextNames;
    public TMP_Text[] LeaderboardTextScores;

    public void UpdateScoreboard(bool showResults = false)
    {
        for (int i = 0; i < GameNFO.EntityNFOs.Length; ++i)
        {
            EntityInformation nfo = GameNFO.EntityNFOs[i];
            if (nfo.PlayerGamepad != null)
                LeaderboardTextNames[i].text = $"Player {nfo.EntityId + 1}";
            else
                LeaderboardTextNames[i].text = $"AI {nfo.EntityId - EntityInformation.CurrentHumanPlayers + 1}";

            LeaderboardTextScores[i].text = showResults ? nfo.Score.ToString() : "?????";

            if (showResults) switch (i)
            {
                case 0:
                    LeaderboardTextNames[i].color = new Color32(252, 202, 3, 255);
                    LeaderboardTextScores[i].color = new Color32(252, 202, 3, 255);
                    break;
                case 1:
                    LeaderboardTextNames[i].color = new Color32(242, 242, 242, 255);
                    LeaderboardTextScores[i].color = new Color32(242, 242, 242, 255);
                    break;
                case 2:
                    LeaderboardTextNames[i].color = new Color32(224, 114, 18, 255);
                    LeaderboardTextScores[i].color = new Color32(224, 114, 18, 255);
                    break;
                default:
                    LeaderboardTextNames[i].color = new Color(0.7f, 0.7f, 0.7f);
                    LeaderboardTextScores[i].color = new Color(0.7f, 0.7f, 0.7f);
                    break;
            }
        }
    }
}
