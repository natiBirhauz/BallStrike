using UnityEngine;

public class PointsBall : MonoBehaviour
{
    private EnemySpawner spawner;
    private Rigidbody rb;
    public float forwardSpeed = 500f;
    private GameManager gameManager;
    private bool hasBeenHit = false; // Flag to track if the ball has been hit

    public void Initialize(EnemySpawner spawner)
    {
        this.spawner = spawner;
        gameManager = Object.FindFirstObjectByType<GameManager>();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.back * forwardSpeed * Time.deltaTime, ForceMode.Force);
    }

    void FixedUpdate()
    {
        if (rb == null) return;
        rb.AddForce(Vector3.back * forwardSpeed * Time.deltaTime, ForceMode.Force);

        if (transform.position.y < -5f || transform.position.z < spawner.player.position.z - 20f)
        {
            Respawn();
        }
    }

    public void Hit()
    {
        // Prevent multiple hits
        if (hasBeenHit) return;
        hasBeenHit = true;
        
        if (gameManager != null)
        {
            gameManager.increaceScore(100000);
        }
        Respawn();
    }

    public void Respawn()
    {
        Destroy(gameObject);
        spawner.ReplaceBall();
    }
}