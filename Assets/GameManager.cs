using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public Health healthUI;
    public Score scoreUI;
    public Player player;
    public int playerHealth = 3;
     public float playerScore = 0;
     public int level=1;
     public float checkPointZPosition = 0f; // Checkpoint position
     public EnemySpawner enemySpawner; // Reference to the enemy spawner

     public GameObject LevelUpCanvas; // Assign a UI panel in the Inspector
    public static GameManager Instance;

    
    void Start()
    {
        if (healthUI == null)
            healthUI = Object.FindFirstObjectByType<Health>();
        if (scoreUI == null)
            scoreUI = Object.FindFirstObjectByType<Score>();

        Debug.Log("GameManager started. Health and Score UI initialized.");
        enemySpawner = Object.FindFirstObjectByType<EnemySpawner>();
    }

    public void InitializePlayer(Player player)
    {
        this.player = player;
        playerHealth = 3;
        UpdateHealthUI();
    }

    void Update()
    {
        increaceScore((int)player.transform.position.z);
        if (player.transform.position.y<-2)
        {
            PlayerDeath();
        }
    }

    public void DecreasePlayerHealth()
    {
        playerHealth--;
        UpdateHealthUI();

        if (playerHealth <= 0)
        {
            PlayerDeath();
        }
    }
        public void IncreasePlayerHealth()
    {
        playerHealth++;
        UpdateHealthUI();
    }

    private void PlayerDeath()
    {
        Debug.Log("Player has died!");
        playerHealth = 3;
        if (playerScore<100)
        {
            playerScore = 0;
        }
        else
        {
            playerScore -= 1000;
        }
        player.transform.position = new Vector3(0, 0, checkPointZPosition); // Reset position
        UpdateHealthUI();
        UpdateScoreUI();
        //enemySpawner.ResetAllBalls(); // Respawn enemies
    }

    private void UpdateHealthUI()
    {
        if (healthUI != null)
        {
            healthUI.UpdateHealthDisplay(playerHealth);
            Debug.Log("Health updated to: " + playerHealth);
        }
        else
        {
            Debug.LogWarning("Health UI not assigned in GameManager.");
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreUI != null && player != null)
        {
            scoreUI.UpdateScoreDisplay(playerScore);
        }

    }
public void increaceScore(int scoreAmount)
{
    playerScore += scoreAmount;
    
    if (scoreUI != null && player != null)
    {
        scoreUI.UpdateScoreDisplay(playerScore); 
    }
    
    if (playerScore < 0)
    {
        playerScore = 0;
    }
    }
    
    private bool levelUpCooldown = false; // Prevent double triggers
public void levelUp()
{
    if (levelUpCooldown) return;

    levelUpCooldown = true;
    Invoke(nameof(ResetCooldown), 1f);

    LevelUpCanvas.SetActive(true);
    level++;

    TMP_Text numberText = LevelUpCanvas.transform.Find("Number")?.GetComponent<TMP_Text>();
    numberText.text = level.ToString();
    Debug.Log("Level Up! Current Level: " + level);
    Invoke(nameof(HideLevelUpCanvas), 2f);
}

void ResetCooldown()
{
    levelUpCooldown = false;
}

void HideLevelUpCanvas()
{
    var imageComponent = LevelUpCanvas.GetComponent<UnityEngine.UI.Image>();
    if (imageComponent != null)
    {
        imageComponent.color = new Color(1, 1, 1, 0);
    }
    else
    {
        Debug.LogError("Image component not found on LevelUpCanvas!");
    }
}

}
