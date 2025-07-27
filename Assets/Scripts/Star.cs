using UnityEngine;

public class Star : MonoBehaviour
{
    private StarSpawner spawner;
    private GameManager gameManager;
    private bool initialized = false;
    private bool hasHit = false; // Prevent double Hit call

    public void Initialize(StarSpawner spawner)
    {
        this.spawner = spawner;
        initialized = true;
    }

    void FixedUpdate()
    {
        if (!initialized || spawner == null || spawner.player == null) return;

        // Despawn if below Y or far behind the player
        if (transform.position.y < -5f || transform.position.z < spawner.player.position.z - 20f)
        {
            Despawn();
        }
    }

    public void Hit()
    {
        if (hasHit) return;
        hasHit = true;

        if (gameManager == null)
        {
            gameManager = Object.FindFirstObjectByType<GameManager>();
        }
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.StarSource);
        gameManager?.AddPoints(500);
        Despawn();
    }

    private void Despawn()
    {
        if (spawner != null)
        {
            spawner.NumOfStars--;
        }
        Destroy(gameObject);
    }
}
