using UnityEngine;

public class HealthBall : MonoBehaviour
{
    private EnemySpawner spawner;
    public float forwardSpeed = 10f;
    private bool initialized = false;
    private GameManager gameManager;

    public void Initialize(EnemySpawner spawner)
    {
        this.spawner = spawner;
        initialized = true;
    }

    void FixedUpdate()
    {
        if (!initialized || spawner == null || spawner.player == null||!GameStateManager.Instance.IsPlaying()) return;

        transform.position += Vector3.back * forwardSpeed * Time.deltaTime;

        if (transform.position.y < -5f || transform.position.z < spawner.player.position.z - 20f)
        {
            Respawn();
        }
    }

    public void Hit()
    {
        if (gameManager == null)
        gameManager = Object.FindFirstObjectByType<GameManager>();
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.helthBallSource);
        gameManager?.IncreasePlayerHealth(1);
        Debug.Log("HealthBall Hit! Player health increased.");
        Respawn();
    }

    public void Respawn()
    {
        Destroy(gameObject);
        spawner?.ReplaceBall();
    }
}