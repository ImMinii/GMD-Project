using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryUI; // drag the InventoryUI GameObject here
    private bool isOpen = false;

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        isOpen = !isOpen;
        inventoryUI.SetActive(isOpen);

        // Optional: lock or unlock player movement
        Time.timeScale = isOpen ? 0 : 1; // pause game while inventory is open
    }
}