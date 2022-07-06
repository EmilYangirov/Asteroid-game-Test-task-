using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;


    private void Start()
    {
        ChangeScoreText();
    }

    public void IncreaseScore(int value)
    {
        score += value;
        ChangeScoreText();
    }

    private void ChangeScoreText()
    {
        scoreText.text = "Очки: " + score;
    }

    public void SaveScore()
    {
        if (!PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", score);
            return;
        }

        if(PlayerPrefs.GetInt("MaxScore") < score)
            PlayerPrefs.SetInt("MaxScore", score);
    }
}
