using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScore : MonoBehaviour
{
    static List<int> levelScores = new List<int>();
    private string levelName;

    public void Start()
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string keyForHighScorePrefs = "Level" + i + "HS";
            PlayerPrefs.GetInt(keyForHighScorePrefs, 0);
        }
    }
    public int GetScore(int index)
    {
        return levelScores[index];
    }
    public string GetName()
    {
        return levelName;
    }
    public void SetHighscore(int index, int score)
    {
        string keyForHighScorePrefs = "Level" + index + "HS";
        if (PlayerPrefs.GetInt(keyForHighScorePrefs, 0) < score)
        {
            PlayerPrefs.SetInt(keyForHighScorePrefs, score);
        }
    }
}
