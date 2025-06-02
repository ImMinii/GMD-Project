using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Jumping jump;
    private WallJumping wallJump;
    private float verticalInput;
    private float previousVerticalInput;
    private PlayerPowerupHandler powerHandler;
    private InventoryToggle inventory;
    private TogglePause pause;

    
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        jump = GetComponent<Jumping>();
        wallJump = GetComponent<WallJumping>();
        powerHandler = GetComponent<PlayerPowerupHandler>();
        inventory = FindFirstObjectByType<InventoryToggle>();
        pause = FindFirstObjectByType<TogglePause>();
    }
    
    private bool IsInputBlocked()
    {
        // Use static bool from InventoryToggle and static Time.timeScale check for pause
        return InventoryToggle.isOpen || Time.timeScale == 0;
    }

    
    public void Move(InputAction.CallbackContext context)
    {
        if (IsInputBlocked()) return;
        Vector2 input = context.ReadValue<Vector2>();
        playerMovement.SetHorizontal(input.x);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (IsInputBlocked()) return;
        if (context.started)
        {
            jump.TryJump(true);
            wallJump.TryWallJump(true);
        }
        else
        {
            jump.TryJump(false);
        }
    }

    public void UsePowerup(InputAction.CallbackContext context)
    {
        if (IsInputBlocked()) return;
        if (context.started)
        {
            powerHandler.UsePowerup();
        }
    }


    public void Inventory(InputAction.CallbackContext context)
    {
        inventory.OnToggleInventory(context);
    }
    
    
    public void Pause(InputAction.CallbackContext context)
    {
        Debug.Log("trigger pause");
        pause.OnTogglePaused(context);
    }
    
}
