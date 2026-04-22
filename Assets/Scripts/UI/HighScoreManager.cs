using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public HighScoreManager instance;
    public List<HighScoreEntry> highScores = new List<HighScoreEntry>();

    private const int MAX_HIGH_SCORES = 5;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsHighScore(int score) 
    {
        if (highScores.Count < MAX_HIGH_SCORES) return true;
        return score > highScores[highScores.Count - 1].score;
    }

    public void AddHighScore(HighScoreEntry newEntry)
    {
        highScores.Add(newEntry);
        highScores.Sort((a, b) => b.score.CompareTo(a.score)); // Sort descending by score
        if (highScores.Count > MAX_HIGH_SCORES)
        {
            highScores.RemoveAt(highScores.Count - 1); // Remove lowest score
        }
        SaveScores();
    }

    public List<HighScoreEntry> GetHighScores()
    {
        return highScores;
    }

    private void SaveScores()
    {
        string json = JsonUtility.ToJson(new HighScoreList { highScores = this.highScores });
        PlayerPrefs.SetString("HighScores", json);      
        PlayerPrefs.Save();
    }
    
    private void LoadScores()
    {
        if (!PlayerPrefs.HasKey("HighScores")) return;

            string json = PlayerPrefs.GetString("HighScores");
            HighScoreList loadedScores = JsonUtility.FromJson<HighScoreList>(json);
       
        if (loadedScores != null)
        {
            highScores = loadedScores.highScores;
        }
    }

    [System.Serializable]
    private class HighScoreList
    {
        public List<HighScoreEntry> highScores;
    }
}
