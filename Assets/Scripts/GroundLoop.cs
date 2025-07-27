using UnityEngine;
public class GroundLoop : MonoBehaviour
{
    public GameManager gameManager;
    public float groundSize = 1000f; // Your chosen ground size
    private bool canTrigger = true;
    private Transform ground1;
    private Transform ground2;

    void Start()
    {
        if (gameManager == null)
        {
            gameManager = Object.FindFirstObjectByType<GameManager>();
        }

        ground1 = GameObject.Find("Ground1")?.transform;
        ground2 = GameObject.Find("Ground2")?.transform;

        if (ground1 == null || ground2 == null)
        {
            Debug.LogError("Ground1 or Ground2 not found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canTrigger || other.CompareTag("Player") == false || gameManager == null) return;

        canTrigger = false;
        gameManager.checkPointZPosition = transform.position.z;
        Transform currentGround = transform.parent;
        float newZ = currentGround.position.z + groundSize;
        currentGround.position = new Vector3(0, 0, newZ);
        
        // Recolor table and walls on the moved ground
        Transform table = currentGround.Find("Table");
        Transform wallR = currentGround.Find("WallR");
        Transform wallL = currentGround.Find("WallL");

        if (table && wallR && wallL)
        {
            Color newColor = new Color(Random.value, Random.value, Random.value);
            table.GetComponent<Renderer>().material.color = newColor;
            wallR.GetComponent<Renderer>().material.color = newColor;
            wallL.GetComponent<Renderer>().material.color = newColor;
        }

        // Corrected: Set checkpoint to THIS trigger's z instead of the moved ground's z
        gameManager.LevelUp();

        Debug.Log($"Checkpoint updated to {gameManager.checkPointZPosition}, Level: {gameManager.level}");

        Invoke(nameof(ResetTrigger), 0.1f);
    }

    private void ResetTrigger()
    {
        canTrigger = true;
    }
}