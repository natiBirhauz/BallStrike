using Unity.VisualScripting; 
using UnityEngine;
public class GroundLoop : MonoBehaviour
{

    public GameManager gameManager; // Reference to the GameManager script
    public float groundSize=1000f; // Size of the ground loop
    public float cooldownTime = 2f; // Adjustable cooldown time in seconds
private bool canTrigger = true; // Flag to control triggering
public void OnTriggerEnter(Collider other)
{
    if (canTrigger && other.CompareTag("Player"))
    {
        canTrigger = false; // Disable triggering
        Transform parentGround = transform.parent;
        Transform Table = null;
        Transform WallR = null;
        Transform WallL = null;

        if (parentGround.name == "Ground1")
        {
            Table = GameObject.Find("Ground1")?.transform.Find("Table");
            WallR = GameObject.Find("Ground1")?.transform.Find("WallR");
            WallL = GameObject.Find("Ground1")?.transform.Find("WallL");
        }
        else if (parentGround.name == "Ground2")
        {
            Table = GameObject.Find("Ground2")?.transform.Find("Table");
            WallR = GameObject.Find("Ground2")?.transform.Find("WallR");
            WallL = GameObject.Find("Ground2")?.transform.Find("WallL");
        }
        Renderer renderer = Table.GetComponent<Renderer>(); 
        Color newColor = new Color(Random.value, Random.value, Random.value);
        renderer.material.color = newColor;

        // Color the walls the same
        Table.GetComponent<Renderer>().material.color= newColor;
        WallR.GetComponent<Renderer>().material.color = newColor;
        WallL.GetComponent<Renderer>().material.color = newColor;
        gameManager.checkPointZPosition +=groundSize;
        transform.parent.position += new Vector3(0, 0, groundSize*2);
        gameManager.levelUp();   
}
   // canTrigger = true; // Re-enable triggering after cooldown
    Invoke(nameof(ResetTrigger), 2f); // Reset trigger after cooldown
}

private void ResetTrigger()
{
    canTrigger = true; // Reset the trigger flag
}
}