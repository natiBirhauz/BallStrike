using UnityEngine;
using System.Collections.Generic;

public enum BallType
{
    Enemy,
    Health,
    Points
}

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyBallPrefab;
    public GameObject healthBallPrefab;
    public GameObject pointsBallPrefab;
    public float healthBallChance = 0.1f;  // 10%
    public float pointsBallChance = 0.2f;  // 20%

    public Transform player;
    private GameManager gameManager;
    private List<GameObject> activeBalls = new List<GameObject>();
    private int lastLevel = 1;

    public float spawnDistanceZ = 100f;
    public float spawnRangeZ = 100f;
    public float spawnRangeX = 4f;
    public float minDistanceBetweenBalls = 5f;

    private Dictionary<BallType, GameObject> ballPrefabs;

    void Start()
    {
        // Initialize ball prefabs dictionary
        ballPrefabs = new Dictionary<BallType, GameObject>
        {
            { BallType.Enemy, enemyBallPrefab },
            { BallType.Health, healthBallPrefab },
            { BallType.Points, pointsBallPrefab }
        };

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

        if (!ballPrefabs.TryGetValue(type, out GameObject prefab))
        {
            Debug.LogError($"No prefab found for ball type: {type}");
            return;
        }

        GameObject newBall = Instantiate(prefab, spawnPos, Quaternion.identity);

        switch (type)
        {
            case BallType.Health:
                if (newBall.TryGetComponent<HealthBall>(out var health))
                {
                    health.Initialize(this);
                }
                break;
            case BallType.Enemy:
                if (newBall.TryGetComponent<EnemyBall>(out var enemy))
                {
                    enemy.Initialize(this);
                }
                break;
            case BallType.Points:
                if (newBall.TryGetComponent<PointsBall>(out var points))
                {
                    points.Initialize(this);
                }
                break;
        }

        activeBalls.Add(newBall);
    }

    public void ReplaceBall()
    {
        // Randomly select ball type based on probability
        float randomValue = Random.value;
        BallType type;

        if (randomValue < healthBallChance)
        {
            type = BallType.Health;
        }
        else if (randomValue < healthBallChance + pointsBallChance)
        {
            type = BallType.Points;
        }
        else
        {
            type = BallType.Enemy;
        }

        ReplaceBall(type);
    }

    private void InitializeBallPool(int count)
    {
        for (int i = 0; i < count * 5; i++)
        {
            // Use the overloaded ReplaceBall method that handles random selection
            ReplaceBall();
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