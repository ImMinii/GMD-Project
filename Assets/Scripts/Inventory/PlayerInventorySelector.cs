using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerInventorySelector : MonoBehaviour
{
    public int playerId;
    public RectTransform selectorVisual;
    public List<InventorySlot> slots;
    private int currentIndex = 0;

    void Start()
    {
        foreach (var slot in slots)
            slot.Refresh();

        MoveTo(currentIndex);
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 move = context.ReadValue<Vector2>();
        int dir = move.y > 0 ? -1 : move.y < 0 ? 1 : 0;

        if (dir != 0)
            ChangeIndex(dir);
    }

    private void ChangeIndex(int direction)
    {
        int original = currentIndex;

        do
        {
            currentIndex = (currentIndex + direction + slots.Count) % slots.Count;
        } while (
            !IsSelectable(slots[currentIndex]) &&
            currentIndex != original
        );

        MoveTo(currentIndex);
    }

    private bool IsSelectable(InventorySlot slot)
    {
        return InventoryController.Instance.IsItemCollected(slot.itemId) && !slot.isPicked;
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        var slot = slots[currentIndex];
        if (IsSelectable(slot))
        {
            slot.LockSlot();
            // Save chosen item to GameManager/PlayerData if needed
        }
    }

    private void MoveTo(int index)
    {
        selectorVisual.position = slots[index].transform.position;
    }
}