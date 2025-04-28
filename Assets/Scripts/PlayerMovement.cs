using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float speed = 4f;

    private float horizontal;
    private bool isFacingRight = true;

    private WallJumping wallJumping;
    [SerializeField] private float wallJumpLockTime = 0.2f; // time after wall jump where movement is locked
    private float wallJumpLockCounter;

    private void Awake()
    {
        wallJumping = GetComponent<WallJumping>();
    }
    
    public void SetHorizontal(float value)
    {
        horizontal = value;
    }

    private void Update()
    { 
        bool onWall = wallJumping.IsOnWall();

        if (wallJumpLockCounter > 0)
        {
            wallJumpLockCounter -= Time.deltaTime;
        }
        
        if (!onWall && wallJumpLockCounter <= 0)  
        {
            body.linearVelocity = new Vector2(horizontal * speed, body.linearVelocity.y);
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
        // Flip only if facing the opposite way
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
