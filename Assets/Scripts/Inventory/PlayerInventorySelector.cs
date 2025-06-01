using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInventorySelector : MonoBehaviour
{
    [Header("Player Reference")]
    public GameObject playerObject; // Assign your player GameObject here (not strictly required but nice for expansion)

    [Header("Selector Visual")]
    public RectTransform selectorVisual; // Assign the selector box (UI Image) here

    [Header("Inventory Slots (row-major order)")]
    public List<InventorySlot> slots; // Drag all 6 slots here: 0-2 top row, 3-5 bottom row

    [Header("Grid Size")]
    public int columns = 3;
    public int rows = 2;

    private int currentIndex = 1;

    void Start()
    {
        foreach (var slot in slots)
            slot.Refresh();

        MoveTo(currentIndex);
    }

    // This method should be bound to the UI/Navigate input action
    public void OnNavigate(InputAction.CallbackContext context)
    {
        Debug.Log("OnNavigate called with: " + context.ReadValue<Vector2>());

        if (!InventoryToggle.isOpen) return;
        if (!context.performed) return;

        Vector2 move = context.ReadValue<Vector2>();
        if (move == Vector2.zero) return;

        int newIndex = currentIndex;

        // Handle left/right movement (columns)
        if (move.x != 0)
        {
            int col = currentIndex % columns;
            int row = currentIndex / columns;
            int newCol = Mathf.Clamp(col + (move.x > 0 ? 1 : -1), 0, columns - 1);
            newIndex = row * columns + newCol;
        }
        // Handle up/down movement (rows)
        else if (move.y != 0)
        {
            int col = currentIndex % columns;
            int row = currentIndex / columns;
            int newRow = Mathf.Clamp(row + (move.y < 0 ? 1 : -1), 0, rows - 1); // Up = -1, Down = +1
            newIndex = newRow * columns + col;
        }

        // Only move if new index is different and the slot is selectable
        if (newIndex != currentIndex && IsSelectable(slots[newIndex]))
        {
            currentIndex = newIndex;
            MoveTo(currentIndex);
        }
    }

    private bool IsSelectable(InventorySlot slot)
    {
        return InventoryController.Instance.IsItemCollected(slot.itemId) && !slot.isPicked;
    }

    // This method should be bound to the UI/Submit input action
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!InventoryToggle.isOpen) return;
        if (!context.performed) return;

        var slot = slots[currentIndex];
        if (IsSelectable(slot))
        {
            slot.LockSlot();
            // Optionally: Add logic here to track who picked what, or trigger events
        }
    }

    private void MoveTo(int index)
    {
        Debug.Log($"Moving selector to slot {index}");
        if (selectorVisual != null && slots[index] != null)
        {
            selectorVisual.anchoredPosition = ((RectTransform)slots[index].transform).anchoredPosition;
            Debug.Log("New anchoredPosition: " + selectorVisual.anchoredPosition);
        }
        else
        {
            Debug.LogWarning("Selector or slot missing!");
        }
    }

}
