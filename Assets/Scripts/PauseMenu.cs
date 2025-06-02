using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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

        if (isOpen)
        {
            // Close inventory if open
            var inventory = GameObject.FindObjectOfType<InventoryToggle>();
            if (inventory != null && InventoryToggle.isOpen)
            {
                inventory.inventoryUI.SetActive(false);
                InventoryToggle.isOpen = false;
            }
        }
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