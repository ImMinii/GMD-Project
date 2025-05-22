using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InputActionReference inventoryToggleAction;
    [SerializeField] private GameObject inventoryUI;

    private bool isInventoryOpen = false;

    private void Awake()
    {
        if (inventoryToggleAction?.action == null)
            Debug.LogError("InputActionReference is not set!");

        if (inventoryUI == null)
            Debug.LogError("Inventory UI GameObject is not assigned.");
    }

    private void Start()
    {
        if (inventoryToggleAction?.action == null)
        {
            Debug.LogError("inventoryToggleAction or its action is NULL!");
        }
        else
        {
            inventoryToggleAction.action.performed += ToggleInventory;
        }

        if (inventoryUI == null)
        {
            Debug.LogError("inventoryUI is NULL!");
        }
    }
    
    public void ToggleInventory(InputAction.CallbackContext context)
    {
        isInventoryOpen = !isInventoryOpen;

        if (inventoryUI == null)
        {
            Debug.LogError("inventoryUI is null!");
            return;
        }

        inventoryUI.SetActive(isInventoryOpen);
    }
}