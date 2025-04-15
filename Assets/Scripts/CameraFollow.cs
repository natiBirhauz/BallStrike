using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the player in the inspector
    public Vector3 offset; // Set this to control the distance between the camera and player

    void Update()
    {
        // Follow the player with the set offset
        transform.position = player.position + offset;
    }
}
