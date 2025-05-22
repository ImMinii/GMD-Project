using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private GroundCheck groundCheck;

    private bool hasDoubleJumped = false;
    private PlayerPowerupHandler powerupHandler;

    private void Awake()
    {
        powerupHandler = GetComponent<PlayerPowerupHandler>();
    }

    public void TryJump(bool jumpPressed)
    {
        if (!jumpPressed) return;

        if (groundCheck.IsGrounded())
        {
            Jump();
            hasDoubleJumped = false; // reset on ground
        }
        else if (powerupHandler != null && powerupHandler.CanDoubleJump() && !hasDoubleJumped)
        {
            Jump();
            hasDoubleJumped = true; // use the double jump
        }
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, 0f); // reset Y velocity
        body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}