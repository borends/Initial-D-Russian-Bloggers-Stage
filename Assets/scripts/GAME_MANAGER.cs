using UnityEngine;
using UnityEngine.SceneManagement;

public class GAME_MANAGER : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("INTERFACE_TEST");
    }
}
