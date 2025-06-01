using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float maxSpeed = 10f;

    private float horizontal;
    private bool isFacingRight = true;
    private Vector2 currentPlatformVelocity = Vector2.zero;

    private WallJumping wallJumping;
    [SerializeField] private float wallJumpLockTime = 0.2f;
    private float wallJumpLockCounter;

    private void Awake()
    {
        wallJumping = GetComponent<WallJumping>();
    }

    public void SetHorizontal(float value)
    {
        horizontal = value;
    }

    public void SetPlatformVelocity(Vector2 velocity)
    {
        currentPlatformVelocity = velocity;
    }

    private void FixedUpdate()
    {
        bool onWall = wallJumping.IsOnWall();
        
        if (InventoryToggle.isOpen) return;

        if (wallJumpLockCounter > 0)
        {
            wallJumpLockCounter -= Time.deltaTime;
        }

        if (!onWall && wallJumpLockCounter <= 0)
        {
            float targetSpeed = horizontal * maxSpeed;
            float speedDiff = targetSpeed - (body.linearVelocity.x - currentPlatformVelocity.x);
            float movement = speedDiff * acceleration * Time.fixedDeltaTime;
            body.AddForce(new Vector2(movement, 0), ForceMode2D.Force);
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

    public void ForceFlip(int direction)
    {
        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public bool IsFacingRight() => isFacingRight;

    public void TriggerWallJumpLock()
    {
        wallJumpLockCounter = wallJumpLockTime;
    }
}
