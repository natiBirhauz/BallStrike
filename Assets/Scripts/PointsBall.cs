using UnityEngine;

public class PointsBall : MonoBehaviour
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
        if (!initialized || spawner == null || spawner.player == null ||!GameStateManager.Instance.IsPlaying()) return;

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
        gameManager.AddPoints(200);
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.pointsBallSource);
        Debug.Log("PointsBall Hit! Points added.");
        Respawn();
    }

    public void Respawn()
    {
        Destroy(gameObject);
        spawner?.ReplaceBall();
    }
}