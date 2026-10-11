using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TMP_Text endTitleText;
    [SerializeField] private TMP_Text endScoreText;

    private int score;
    public bool IsPlaying { get; private set; } = true;

    private void OnEnable()
    {
        Enemy.Killed += AddScore;
    }

    private void OnDisable()
    {
        Enemy.Killed -= AddScore;
    }

    private void Start()
    {
        endPanel.SetActive(false);
        UpdateScoreText();
    }
    public void AddScore(int points)
    {
        if (!IsPlaying)
        {
            return;
        }

        score += points;
        UpdateScoreText();
    }
    public void EndGame(bool won)
    {
        if (!IsPlaying)
        {
            return;
        }

        IsPlaying = false;
        bool isNewBest = HighScoreStorage.TrySetHighScore(score);

        endTitleText.text = won ? "You Win!" : "Game Over";
        endScoreText.text = isNewBest
            ? $"Score: {score}\nNew High Score!"
            : $"Score: {score}\nBest: {HighScoreStorage.HighScore}";

        endPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }
}
