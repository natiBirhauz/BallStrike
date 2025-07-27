using UnityEngine; 

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab;
    public Transform player;
    public float spawnDistanceZ = 100f;
    public float spawnRangeZ = 100f;
    public float spawnRangeX = 4f;
    public float minDistanceBetweenStars = 3f;
    public int NumOfStars = 0;
    public int MaxNumOfStars = 3;

    private float spawnCooldown = 0.5f;
    private float timeSinceLastSpawn = 0f;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    void Update()
    {
        if (player == null || starPrefab == null||!GameStateManager.Instance.IsPlaying()) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (NumOfStars < MaxNumOfStars && timeSinceLastSpawn >= spawnCooldown)
        {
            SpawnStar();
            timeSinceLastSpawn = 0f;
        }
    }

    public void SpawnStar()
    {
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        float z = player.position.z + spawnDistanceZ + Random.Range(0, spawnRangeZ);
        Vector3 spawnPos = new Vector3(x, 1.5f, z);

        GameObject starObj = Instantiate(starPrefab, spawnPos, Quaternion.identity);
        Star star = starObj.GetComponent<Star>();
        if (star != null)
        {
            star.Initialize(this);
        }
        NumOfStars++;
    }

    internal void ResetStars()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        NumOfStars = 0;
        timeSinceLastSpawn = 0f;

    }
}
