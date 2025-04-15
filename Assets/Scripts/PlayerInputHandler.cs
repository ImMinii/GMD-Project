using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Jumping jump;
    private WallJumping wallJump;
    private float verticalInput;
    private float previousVerticalInput;
    
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        jump = GetComponent<Jumping>();
        wallJump = GetComponent<WallJumping>();
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
            jump.TryJump(true); // Normal jump
            wallJump.TryWallJump(true); // Wall jump attempt
        }
        else
        {
            jump.TryJump(false);
        }
    }
    
}
