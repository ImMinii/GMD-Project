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
        
        previousVerticalInput = verticalInput;
        verticalInput = input.y;

        bool upJustPressed = previousVerticalInput <= 0 && verticalInput > 0;

        if (upJustPressed)
        {
            jump.TryJump(true); // Regular jump
            wallJump.TryWallJump(verticalInput, true); // Wall jump
        }
        else
        {
            jump.TryJump(false);
        }
    }
    
}
