using TMPro;
using UnityEngine;

public class MainMenuDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;

    private void Start()
    {
        highScoreText.text = $"High Score: {HighScoreStorage.HighScore}";
    }
}
