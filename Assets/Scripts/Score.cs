using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        if (scoreText == null)
            scoreText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScoreDisplay(float score)
    {
        scoreText.SetText("Score: " + score.ToString("0"));
    }
}
