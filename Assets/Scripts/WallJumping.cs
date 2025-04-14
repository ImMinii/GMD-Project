using UnityEngine;

public class WallJumping : MonoBehaviour
{ 
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);

    [SerializeField] private Vector2 wallJumpPower = new Vector2(4f, 6f);
    [SerializeField] private int maxWallJumps = 2;
    [SerializeField] private float pushAwayForce = 2f;

    private bool isWallSticking;
    private int wallJumpCount;

    private GroundCheck groundCheck;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        groundCheck = GetComponent<GroundCheck>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        bool onWall = IsOnWall();
        bool grounded = groundCheck.IsGrounded();

        if (onWall && !grounded)
        {
            isWallSticking = true;
            if (body.linearVelocity.y < 0)
            {
                body.linearVelocity = new Vector2(0f, 0f);
            }
            wallJumpCount = maxWallJumps;
                         
        }
        else
        {
            isWallSticking = false;
            
            if (grounded)
                wallJumpCount = maxWallJumps;
        }
    }

    public void TryWallJump(float verticalInput, bool jumpPressed)
    {
        if (isWallSticking && jumpPressed && verticalInput > 0 && wallJumpCount > 0)
        {
            wallJumpCount--;

            int direction = playerMovement.IsFacingRight() ? -1 : 1;
            
            Vector2 jumpForce = new Vector2(direction * wallJumpPower.x, wallJumpPower.y);
            body.linearVelocity = jumpForce;
            body.AddForce(new Vector2(direction * pushAwayForce, 0f), ForceMode2D.Impulse);
            
            FlipIfNeeded(direction);
            isWallSticking = false;
            
        }
    }

    public bool IsOnWall()
    {
        return Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer);
    }

    private void FlipIfNeeded(int jumpDirection)
    {
        bool shouldFlip = (playerMovement.IsFacingRight() && jumpDirection > 0) ||
                          (!playerMovement.IsFacingRight() && jumpDirection < 0);

        if (shouldFlip)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }
}
