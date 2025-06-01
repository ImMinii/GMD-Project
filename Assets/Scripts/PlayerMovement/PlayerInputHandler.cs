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
    

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        playerMovement.SetHorizontal(input.x);
        
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
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

    public void Inventory(InputAction.CallbackContext context)
    {
        inventory.OnToggleInventory(context);
    }
    
    public void UsePowerup(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            powerHandler.UsePowerup();
        }
    }
    
    public void Pause(InputAction.CallbackContext context)
    {
        Debug.Log("trigger pause");
        pause.OnTogglePaused(context);
    }
    
}
