using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIHUD : MonoBehaviour
{
    public void QuitButton()
    {
        Application.Quit();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
