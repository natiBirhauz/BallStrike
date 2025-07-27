using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private void Awake()
    {
        // Initialization code here
        Shader.SetGlobalFloat("_Curvature", 2.0f);
        Shader.SetGlobalFloat("_Trimming", 0.1f);
        Debug.Log("SceneManager awake called");
    }
}
