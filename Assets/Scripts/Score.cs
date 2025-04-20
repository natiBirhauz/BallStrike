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

    private string FormatScore(int score)
    {
        if (score >= 1000000)
            return "Score:"+(score / 1000000f).ToString("0.#") + "M";
        if (score >= 1000)
            return "Score:"+(score / 1000f).ToString("0.#") + "K";
        return score.ToString();
    }

    public void UpdateScoreDisplay(float score)
    {
        int roundedScore = Mathf.FloorToInt(score); // round down to nearest int
        scoreText.text = FormatScore(roundedScore);
    }
}
