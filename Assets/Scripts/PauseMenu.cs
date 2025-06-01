using UnityEngine;
using UnityEngine.InputSystem;

public class TogglePause : MonoBehaviour
{
    public GameObject PausedUI;
    private bool isOpen = false;
    
    
  public void TogglePaused()
    {
        Debug.Log("paused");
        isOpen = !isOpen;
        PausedUI.SetActive(isOpen);
        Time.timeScale = isOpen ? 0 : 1;
    }
    public void OnTogglePaused(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        TogglePaused();
    }
    
  

    public void QuitButton()
    {
        Application.Quit();
        
    }

    public void ResumeButton()
    {
        TogglePaused();
    }
    
    
}
