using UnityEngine;
using System.Collections;

public class MagnetBall : MonoBehaviour
{
    private EnemySpawner spawner;
        private StarSpawner StarSpawner;

    private GameManager gameManager;
    public float forwardSpeed = 10f;
    private bool initialized = false;

    public float magnetRange = 50f;
    public float pullSpeed = 20f;
    public float magnetDuration = 50f;

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
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.magnetBallSource);
        StartCoroutine(ActivateMagnet());
        Respawn();
    }

private IEnumerator ActivateMagnet()
{
    float timer = 0f;
    Transform player = spawner.player;

    while (timer < magnetDuration)
    {
        GameObject[] stars = GameObject.FindGameObjectsWithTag("Star");

        foreach (var starObj in stars)
        {
            if (starObj == null) continue;

            float distance = Vector3.Distance(player.position, starObj.transform.position);

            if (distance <= magnetRange)
            {
                // Smoothly pull the star toward the player
                Vector3 targetPos = new Vector3(player.position.x, starObj.transform.position.y, player.position.z);
                starObj.transform.position = Vector3.MoveTowards(starObj.transform.position, targetPos, pullSpeed * Time.deltaTime);
            }
        }

        timer += Time.deltaTime;
        yield return null;
    }
}




    private void Respawn()
    {
        Destroy(gameObject);
        spawner?.ReplaceBall();
    }
}