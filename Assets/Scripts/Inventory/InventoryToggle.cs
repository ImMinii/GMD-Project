using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryUI; // drag the InventoryUI GameObject here
    public static bool isOpen = false;

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (Time.timeScale == 0) return;
    
        isOpen = !isOpen;
        inventoryUI.SetActive(isOpen);
    
        if (isOpen)
        {
            var selector = GameObject.FindObjectOfType<PlayerInventorySelector>();
            if (selector != null)
                selector.RefreshAllSlots();
        }
    }


}