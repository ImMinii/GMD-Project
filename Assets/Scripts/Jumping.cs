using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private GroundCheck groundCheck;

    public void TryJump(bool jumpPressed)
    {
        if (jumpPressed && groundCheck.IsGrounded())
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
