using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

[Serializable]
public class ScoreEntry
{
    public string name;
    public int score;
}

[Serializable]
public class ScoreList
{
    public List<ScoreEntry> entries = new();
}

public class LeaderboardManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    public Button submitButton;
    public TextMeshProUGUI leaderboardText;
    public GameObject leaderboardPanel;

    private ScoreList scoreList = new();
    private string savePath;
    private GameManager gm;

    void Start()
    {
        gm = FindFirstObjectByType<GameManager>();
        submitButton.onClick.AddListener(OnSubmit);
        savePath = Path.Combine(Application.persistentDataPath, "highscores.json");
        //add scores to leaderboard

        LoadScores(); // Load existing scores at start
        ShowLeaderboard();
    }

    void OnSubmit()
    {
        string playerName = nameInput.text;
        int playerScore = gm.GetPoints();

        if (!string.IsNullOrEmpty(playerName))
        {
            AddScore(playerName, playerScore);
            nameInput.text = "";
            ShowLeaderboard();
        }
    }

    public void AddScore(string name, int score)
    {
        scoreList.entries.Add(new ScoreEntry { name = name, score = score });
        scoreList.entries.Sort((a, b) => b.score.CompareTo(a.score));
        if (scoreList.entries.Count > 10) scoreList.entries.RemoveAt(scoreList.entries.Count - 1);
        SaveScores();
    }

    public void ShowLeaderboard()
    {
        StringBuilder sb = new();
        sb.AppendLine("Leaderboard\n");
        for (int i = 0; i < scoreList.entries.Count; i++)
        {
            sb.AppendLine($"{i + 1}. {scoreList.entries[i].name} - {scoreList.entries[i].score}");
        }

        leaderboardText.text = sb.ToString();
        leaderboardPanel.SetActive(true);
    }

    private void SaveScores()
    {
        string json = JsonUtility.ToJson(scoreList, true);
        File.WriteAllText(savePath, json);
    }

    private void LoadScores()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            scoreList = JsonUtility.FromJson<ScoreList>(json);
        }
    }
}
