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

    [Header("Player Input")]
    public int playerIndex = 0;

    private int currentIndex = 0;
    private float inputCooldown = 0.2f; // prevent fast skipping
    private float inputTimer = 0f;
    
    private bool isActive = false;


    void Start()
    {
        foreach (var slot in slots)
            slot.Refresh();

        MoveTo(currentIndex);
    }
    
    void Update()
    {
        if (!InventoryToggle.isOpen) return;
        if (Gamepad.all.Count <= playerIndex) return;

        inputTimer -= Time.unscaledDeltaTime;
        var gamepad = Gamepad.all[playerIndex];

        // Only allow navigation if not "locked in" to a slot
        if (!isActive)
        {
            Vector2 nav = gamepad.leftStick.ReadValue();
            bool navPressed = nav.magnitude > 0.5f;

            if (navPressed && inputTimer <= 0)
            {
                OnNavigate(nav);
                inputTimer = inputCooldown;
            }
        }

        // Submit or unsubmit regardless
        if (gamepad.buttonSouth.wasPressedThisFrame)
        {
            if (!isActive)
                OnSubmit();
            else
                OnUnsubmit(); // We'll add this next
        }
    }


    public void OnNavigate(Vector2 move)
    {
        if (move == Vector2.zero) return;
        int newIndex = currentIndex;

        if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
        {
            // Horizontal
            int col = currentIndex % columns;
            int row = currentIndex / columns;
            int newCol = Mathf.Clamp(col + (move.x > 0 ? 1 : -1), 0, columns - 1);
            newIndex = row * columns + newCol;
        }
        else
        {
            // Vertical
            int col = currentIndex % columns;
            int row = currentIndex / columns;
            int newRow = Mathf.Clamp(row + (move.y < 0 ? 1 : -1), 0, rows - 1);
            newIndex = newRow * columns + col;
        }

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
// When submitting, lock to the slot and block further movement
    public void OnSubmit()
    {
        var slot = slots[currentIndex];
        if (IsSelectable(slot))
        {
            slot.LockSlot();
            isActive = true;
            slot.ActivatePowerupForPlayer(playerObject);
        }
    }


// When "unsubmitting" (e.g., pressing A/B again), unlock and allow movement
    public void OnUnsubmit()
    {
        var slot = slots[currentIndex];
        slot.UnlockSlot(); // Optional: if you want toggle
        isActive = false;
    }


    private void MoveTo(int index)
    {
        if (selectorVisual != null && slots[index] != null)
        {
            selectorVisual.anchoredPosition = ((RectTransform)slots[index].transform).anchoredPosition;
        }
    }
    
    public void RefreshAllSlots()
    {
        foreach (var slot in slots)
            slot.Refresh();
    }


}
