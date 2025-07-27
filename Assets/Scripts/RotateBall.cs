using UnityEngine;

public class RotatePreview : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 30f, 0); // 30 degrees/sec around Y

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
