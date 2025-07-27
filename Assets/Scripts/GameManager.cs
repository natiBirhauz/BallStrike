using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int level = 1;
    private float pointsTimer = 0f;
    private float pointsInterval = 1f;
    public int points = 0;
    public int playerHealth = 3;
    private Player player;
    private bool isInitialized = false;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI scoreTextPaused;
    public TextMeshProUGUI healthTextpaused;
    public TextMeshProUGUI LevelTextpaused;
    public float checkPointZPosition = 0f; // Default checkpoint Z position
    public GameStateManager gameStateManager;
public List<Material> skyboxMaterials; // drag AllSkyFree skyboxes here
private int currentSkyboxIndex = 0;
    
    //2 gamemode modes
    public enum GameMode
    {
        Normal,
        endless
    }
    public GameMode currentGameMode = GameMode.Normal;

    void Awake()
    {
        // Ensure initialization is done early
        player = Object.FindFirstObjectByType<Player>();
        if (player != null)
        {
            InitializePlayer(player);
            Debug.Log("Player found in GameManager.Awake");
        }
        else
        {
            Debug.LogWarning("Player not found in GameManager.Awake!");
        }
        isInitialized = true;
    }

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ChangeBackground(); // Press B to cycle skybox
        Debug.Log("Changing skybox to: " + skyboxMaterials[currentSkyboxIndex].name);

    }
        if (!isInitialized || player == null)
            return;

        if (GameStateManager.Instance.IsPlaying())
        { 
        pointsTimer += Time.deltaTime;
        if (pointsTimer >= pointsInterval)
        {
            AddPoints(1);
            pointsTimer -= pointsInterval;
        }
    }
        if (Input.GetKeyDown(KeyCode.Space) && gameStateManager != null)
        {
                    if (gameStateManager.CurrentState == GameState.Playing)
        {
            gameStateManager.SetGameState(GameState.Paused);
        }
            if (gameStateManager.CurrentState == GameState.Paused)
            {
                gameStateManager.SetGameState(GameState.Playing);
            }

        }
        
        
    }

    public void LevelUp()
    {
        if (!isInitialized) return;
        level++;
        AddPoints(100);
        player.forwardSpeed = Mathf.Max(0, player.forwardSpeed + 5f);
        player.baseSpeed = player.forwardSpeed; // Update base speed to match new forward speed
        Debug.Log($"Level Up! Current Level: {level}, Points: {points}");
            ChangeBackground();

    }

    public void ChangeBackground()
    {
    if (skyboxMaterials == null || skyboxMaterials.Count == 0)
    {
        Debug.LogWarning("No skybox materials assigned!");
        return;
    }

    currentSkyboxIndex = (currentSkyboxIndex + 1) % skyboxMaterials.Count;
    RenderSettings.skybox = skyboxMaterials[currentSkyboxIndex];

    // Optional: update ambient lighting based on new skybox
    DynamicGI.UpdateEnvironment();
}

    public void AddPoints(int amount)
    {
        points += amount;
        UpdateUI();
    }

    public void DecreasePoints(int amount)
    {
        points = Mathf.Max(0, points - amount);
        UpdateUI();
    }

    public void IncreasePlayerHealth(int amount)
    {
        playerHealth += amount;
        UpdateUI();
    }

    public void DecreasePlayerHealth()
    {
        playerHealth = Mathf.Max(0, playerHealth - 1);
        if (playerHealth <= 0)
        {
            HandlePlayerRespawn();
        }

        UpdateUI();
    }
        public void DecreasePlayerSpeed()
    {
        player.forwardSpeed = Mathf.Max(0, player.forwardSpeed / 2f);
        Invoke(nameof(ResetPlayerSpeed), 0.5f); // Reset speed after 2 seconds
        UpdateUI();
    }
    public void IncreasePlayerrSpeed()
    {
        player.forwardSpeed = Mathf.Max(0, player.forwardSpeed * 1.5f);
        Invoke(nameof(ResetPlayerSpeed), 0.5f); // Reset speed after 2 seconds
        UpdateUI();
    }

    private object ResetPlayerSpeed()
    {
        player.forwardSpeed = player.baseSpeed; // Reset to base speed
        return null;
    }


    private void HandlePlayerRespawn()
    {

  if (currentGameMode == GameMode.endless)
    {
        DecreasePoints(100);
        playerHealth = 3;

        if (player != null)
            player.transform.position = new Vector3(0, 1, checkPointZPosition);
        else
            Debug.LogWarning("Player not found during respawn!");

        UpdateUI();
    }
    else if (currentGameMode == GameMode.Normal)
    {
        gameStateManager.ChangeToDeathMenu();
    }
        
    }

    public void InitializePlayer(Player player)
    {
        this.player = player;
    }

    public bool IsInitialized()
    {
        return isInitialized && player != null;
    }

    public int GetPoints() => points;
    public int GetHealth() => playerHealth;

    public void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {points}";
            scoreTextPaused.text = $"Score: {points}";
        }
        else
        {
            Debug.LogWarning("Score Text UI not assigned!");
        }

        if (healthText != null)
        {
            healthText.text = $"Health: {playerHealth}";
            healthTextpaused.text = $"Health: {playerHealth}";

        }
        else
        {
            Debug.LogWarning("Health Text UI not assigned!");
        }
        if (LevelText != null)
        {
            LevelText.text = $"Level: {level}";
            LevelTextpaused.text = $"Level: {level}";
        }
        else
        {
            Debug.LogWarning("Level Text UI not assigned!");
        }
    }
}
