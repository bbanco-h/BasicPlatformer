using UnityEngine;

public static class HighScoreStorage
{
    private const string HighScoreKey = "HighScore";
    public static int HighScore => PlayerPrefs.GetInt(HighScoreKey, 0);
    public static bool TrySetHighScore(int score)
    {
        if (score <= HighScore)
        {
            return false;
        }

        PlayerPrefs.SetInt(HighScoreKey, score);
        PlayerPrefs.Save();
        return true;
    }
}
