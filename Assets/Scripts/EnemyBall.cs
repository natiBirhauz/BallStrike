using UnityEngine;

public class EnemyBall : MonoBehaviour
{
    private EnemySpawner spawner;
    private Rigidbody rb;
    public float forwardSpeed = 500f;
    private GameManager gameManager;

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
        if (gameManager != null)
        {
            gameManager.DecreasePlayerHealth();
        }

        Respawn();
    }

    public void Respawn()
    {
        Destroy(gameObject);
        spawner.ReplaceBall();
    }
}
