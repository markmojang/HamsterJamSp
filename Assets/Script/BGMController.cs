using UnityEngine;

public class BGMController : MonoBehaviour
{
    private static BGMController instance;

    void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate BGM GameObject
        }
    }
}