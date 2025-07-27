using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
    Options,
    Leaderboard,
    deathMenu
}

public class GameStateManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject leaderboardMenu;
    public GameObject gameOverMenu;
    public GameObject pauseMenu;
    public GameObject playingMenu;
    public GameObject deathMenu;
    public GameObject Animetions;
    public float delay = 0.5f; // Delay before changing state

    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.MainMenu;
    public GameState previousState { get; private set; } = GameState.MainMenu;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (transform.parent != null)
        {
            transform.SetParent(null); // Detach from parent
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetGameState(GameState.MainMenu);

    }

    public void SetGameState(GameState newState)
    {
        if (CurrentState != newState)
        {
            StartCoroutine(TransitionToState(newState));
        }
    }

    private IEnumerator TransitionToState(GameState newState)
    {
        yield return new WaitForSecondsRealtime(delay);
        CurrentState = newState;
        HandleGameStateChange();
    }

    private void HandleGameStateChange()
    {
        HideAllMenus();

        switch (CurrentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 0;
                mainMenu.SetActive(true);
                Animetions.SetActive(false);
                previousState = GameState.MainMenu;
                AudioManager.Instance?.PlayMusic(AudioManager.Instance.MenuMusicSource);
                break;
            case GameState.Playing:
                Time.timeScale = 1;
                playingMenu.SetActive(true);
                Animetions.SetActive(true);
                previousState = GameState.Playing;
                AudioManager.Instance?.PlayMusic(AudioManager.Instance.MusicGameLoopSource);
                break;
            case GameState.Paused:
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                Animetions.SetActive(false);
                previousState = GameState.Paused;


                AudioManager.Instance?.PlayMusic(AudioManager.Instance.MenuMusicSource);
                break;
            case GameState.GameOver:
                Time.timeScale = 0;
                gameOverMenu.SetActive(true);
                Animetions.SetActive(false);
                AudioManager.Instance?.PlayMusic(AudioManager.Instance.MenuMusicSource);
                break;
            case GameState.Options:
                Time.timeScale = 0;
                optionsMenu.SetActive(true);
                Animetions.SetActive(false);
                break;
            case GameState.Leaderboard:
                Time.timeScale = 0;
                leaderboardMenu.SetActive(true);
                Animetions.SetActive(false);
                break;
            case GameState.deathMenu:
                Time.timeScale = 0;
                deathMenu.SetActive(true);
                Animetions.SetActive(false);
                break;
        }
    }

    private void HideAllMenus()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        leaderboardMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        playingMenu.SetActive(false);
        deathMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting...");
    }

    public void OpenOptions() => SetGameState(GameState.Options);
    public void OpenLeaderboard() => SetGameState(GameState.Leaderboard);
    public void ChangeToMainMenu() => SetGameState(GameState.MainMenu);
    public void ChangeToGameOver() => SetGameState(GameState.GameOver);
    public void ChangeToPaused() => SetGameState(GameState.Paused);
    public void ChangeToPlaying() => SetGameState(GameState.Playing);
    public void ChangeToDeathMenu() => SetGameState(GameState.deathMenu);
    private Transform ground1;
    private Transform ground2;
    public void RestartGame()
    {
        resetGame();
        SetGameState(GameState.Playing);
    }
    public void resetGame()
    {

        // Reset game state to initial conditions
        GameManager gm = Object.FindFirstObjectByType<GameManager>();
        Player player = Object.FindFirstObjectByType<Player>();
        ground1 = GameObject.Find("Ground1")?.transform;
        ground2 = GameObject.Find("Ground2")?.transform;
        ground1.position = new Vector3(0, 0, 0);
        ground2.position = new Vector3(0, 0, 0);
        player.transform.position = new Vector3(0, 1, 0); // Reset player position
        player.forwardSpeed = player.baseSpeed;
        
            // Fully reset stats
            gm.level = 1;
            gm.playerHealth = 3;
            gm.points = 0;
            gm.UpdateUI();
            player.baseSpeed = 15f;
            player.forwardSpeed = player.baseSpeed;

        //when the player respawns, reset enemy balls
        EnemySpawner spawner = Object.FindFirstObjectByType<EnemySpawner>();
        StarSpawner starSpawner = Object.FindFirstObjectByType<StarSpawner>();
        if (starSpawner != null)
        {
            starSpawner.ResetStars();
        }
        if (spawner != null)
        {
            spawner.ResetEnemyBalls();
        }


        if (gm != null && player != null)
        {
            // Reset game manager stats
            gm.InitializePlayer(player); // Ensure player is registered
            gm.checkPointZPosition = 0f;
            // Reset player position to checkpoint
            player.transform.position = new Vector3(0, 1, gm.checkPointZPosition);
        }          

    }

    public bool IsPlaying()
    {
        return CurrentState == GameState.Playing;
    }
    public void tuggleMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleMusic();
        }
    }
    public void tuggleSfx()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleSFX();
        }
    }
    public void backState()
    {
        SetGameState(previousState);
    }
    public void setGameMode()
    {
        GameManager gm = Object.FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            if (gm.currentGameMode == GameManager.GameMode.Normal)
            {
                gm.currentGameMode = GameManager.GameMode.endless;
                //change text of the button 
                GameObject.Find("GameModeButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Game Mode: Endless";
                
                resetGame();
            }
            else
            {
                gm.currentGameMode = GameManager.GameMode.Normal;
                GameObject.Find("GameModeButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Game Mode: Normal";

                resetGame();

            }

        }
    }

}
