using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text[] entriesText;

    private void OnEnable()
    {
        LoadScores();
    }

    private void LoadScores()
    {
        var scores = HighScoreManager.instance.GetHighScores();

        for (int i = 0; i < entriesText.Length; i++)
        {
            if (i < scores.Count)
            {
                var s = scores[i];
                entriesText[i].text =
                    $"{s.playerName} - {s.score} | {s.livesRemaining}HP | {s.enemiesKilled} Kills";
            }
            else
            {
                entriesText[i].text = $"No data";
            }
        }
    }
}