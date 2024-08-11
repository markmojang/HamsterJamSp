using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void PlaySound(AudioClip clip)
    {
        if (instance != null && instance.audioSource != null)
        {
            instance.audioSource.PlayOneShot(clip);
        }
    }
}