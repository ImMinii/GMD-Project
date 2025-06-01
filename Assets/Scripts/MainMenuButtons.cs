using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{

    public void PlayNowButton()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    

    public void QuitButton()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
