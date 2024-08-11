using UnityEngine;
using UnityEngine.SceneManagement;

public class HardResetButton : MonoBehaviour
{
    public void HardReset()
    {
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
