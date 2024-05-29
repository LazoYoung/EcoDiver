using UnityEngine;

public class ApplicationSettings : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 1f / 60f;
        Debug.Log("Framerate: " + 60);
    }
}
