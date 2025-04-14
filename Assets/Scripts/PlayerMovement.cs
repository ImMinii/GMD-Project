using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float speed = 4f;

    private float horizontal;
    private bool isFacingRight = true;

    private WallJumping wallJumping;

    private void Awake()
    {
        wallJumping = GetComponent<WallJumping>();
    }
    
    public void SetHorizontal(float value)
    {
        horizontal = value;
    }

    private void FixedUpdate()
    { 
        bool onWall = wallJumping.IsOnWall(); // Check if player is on a wall

        if (!onWall)  // If not on a wall, move horizontally as usual
        {
            body.linearVelocity = new Vector2(horizontal * speed, body.linearVelocity.y);
        }
        else
        {
            // Stop horizontal movement when sticking to the wall
            body.linearVelocity = new Vector2(0f, body.linearVelocity.y);
        }
        Flip();
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0 || !isFacingRight && horizontal > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public bool IsFacingRight() => isFacingRight;
}
