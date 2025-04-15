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

        // Wall sticking logic
        if (onWall && !grounded)
        {
            isWallSticking = true;
            
            if (body.linearVelocity.y < 0)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, -1f);
            }
            wallJumpCount = maxWallJumps;
        }
        else
        {
            isWallSticking = false;
            if (grounded) wallJumpCount = maxWallJumps;  // Reset jump count when grounded
        }
    }

    public void TryWallJump(bool jumpPressed)
    {
        if (!isWallSticking || !jumpPressed || wallJumpCount <= 0)
            return;

        wallJumpCount--;

        int direction = playerMovement.IsFacingRight() ? -1 : 1;

        // Apply the wall jump force
        Vector2 jumpForce = new Vector2(-(direction * (wallJumpPower.x + pushAwayForce)), wallJumpPower.y);
        body.linearVelocity = jumpForce;
        Debug.Log("Wall jump direction: " + direction);
        // Flip the character
        playerMovement.ForceFlip(direction);

        isWallSticking = false;
    }

    public bool IsOnWall()
    {
        return Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }
}
