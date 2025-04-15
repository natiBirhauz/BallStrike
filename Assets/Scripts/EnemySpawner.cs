using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyBallPrefab;
    public GameObject healthBallPrefab;
    public float healthBallChance = 0.5f;

    public Transform player;
    private GameManager gameManager;
    private List<GameObject> activeBalls = new List<GameObject>();
    private int lastLevel = 1;

    public float spawnDistanceZ = 100f;
    public float spawnRangeZ = 100f;
    public float spawnRangeX = 4f;
    public float minDistanceBetweenBalls = 5f;

    void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.position = new Vector3(0, 1, 0);
        player.rotation = Quaternion.LookRotation(Vector3.forward);
        InitializeBallPool(gameManager.level);
    }

    void Update()
    {
        if (gameManager.level > lastLevel)
        {
            int additionalBalls = gameManager.level - lastLevel;
            lastLevel = gameManager.level;
            InitializeBallPool(additionalBalls);
        }
    }

public void ReplaceBall(BallType type)
{
    Vector3 spawnPos = GetValidSpawnPosition();

    GameObject prefab = type switch
    {
        BallType.Health => healthBallPrefab,
        BallType.Enemy => enemyBallPrefab,
        _ => enemyBallPrefab
    };

    GameObject newBall = Instantiate(prefab, spawnPos, Quaternion.identity);

    if (type == BallType.Health && newBall.TryGetComponent<HealthBall>(out var health))
    {
        health.Initialize(this);
    }
    else if (type == BallType.Enemy && newBall.TryGetComponent<EnemyBall>(out var enemy))
    {
        enemy.Initialize(this);
    }

    activeBalls.Add(newBall);
}
public void ReplaceBall()
{
    BallType type = Random.value < 0.1f ? BallType.Health : BallType.Enemy;
    ReplaceBall(type);
}


    private void InitializeBallPool(int count)
    {
        for (int i = 0; i < count * 5; i++)
        {
            BallType type = Random.value < healthBallChance ? BallType.Health : BallType.Enemy;
            ReplaceBall(type);
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPos;
        int attempts = 0;
        do
        {
            float x = Random.Range(-spawnRangeX, spawnRangeX);
            float z = player.position.z + spawnDistanceZ + Random.Range(0f, spawnRangeZ);
            spawnPos = new Vector3(x, 1f, z);
            attempts++;
        } while (!IsFarEnough(spawnPos) && attempts < 10);

        return spawnPos;
    }

    private bool IsFarEnough(Vector3 newPos)
    {
        foreach (var obj in activeBalls)
        {
            if (obj == null) continue;
            if (Vector3.Distance(newPos, obj.transform.position) < minDistanceBetweenBalls)
                return false;
        }
        return true;
    }
}
