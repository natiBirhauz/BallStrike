using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the player in the inspector
    public Vector3 offset; // Set this to control the distance between the camera and player

    void Update()
    {

        transform.position = player.position + offset;
    }
}
