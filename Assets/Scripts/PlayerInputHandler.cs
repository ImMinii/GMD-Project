using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float speed = 4f;
    

    private float horizontalMovement;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    
    [SerializeField] float jumpForce = 3f;

    
    public Transform wallCheck;
    public LayerMask wallLayer;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    private bool isFaceingRight = true;
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTimer;
    private float wallJumpTime = 0f;
    public Vector2 wallJumpPower = new Vector2(2F, 4f);
    [SerializeField] private float wallStickDuration = 0.25f;
    private float wallStickTimer;
    private bool isWallSticking;
    
    
    private void Start()
    {
        //body = GetComponent<Rigidbody2D>();
    }

    

    void FixedUpdate()
    {
        bool grounded = IsGrounded();
        bool onWall = isOnWall();

        ProcessWallJump();

        if (onWall && !grounded && horizontalMovement != 0 && !isWallJumping)
        {
            isWallSticking = true;
            wallStickTimer = wallStickDuration;
            body.linearVelocity = Vector2.zero; // Freeze on wall
        }
        else if (wallStickTimer > 0)
        {
            wallStickTimer -= Time.deltaTime;
        }
        else
        {
            isWallSticking = false;
        }

        if (!isWallJumping && !isWallSticking)
        {
            body.linearVelocity = new Vector2(horizontalMovement * speed, body.linearVelocity.y);
            Flip();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;

        // Jump
        if (IsGrounded())
        {
            if (context.ReadValue<Vector2>().y > 0)
            {
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            } 
        }
        
        if (isOnWall())
        {
            if (isWallSticking && context.performed && context.ReadValue<Vector2>().y > 0)
            {
                isWallJumping = true;
                wallJumpDirection = isFaceingRight ? -1 : 1;

                body.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
                wallJumpTimer = wallJumpTime;

                // Flip if jumping opposite direction
                if (isFaceingRight && wallJumpDirection < 0 || !isFaceingRight && wallJumpDirection > 0)
                {
                    isFaceingRight = !isFaceingRight;
                    Vector3 ls = transform.localScale;
                    ls.x *= -1f;
                    transform.localScale = ls;
                }

                isWallSticking = false;
            }
        }
    }

    private bool IsGrounded()
    {
        return (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer));
    }

    private bool isOnWall()
    {
        return (Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer));
    }

    private void Flip()
    {
        if (isFaceingRight && horizontalMovement < 0 || !isFaceingRight && horizontalMovement > 0 )
        {
            isFaceingRight = !isFaceingRight;
            Vector3 ls = transform.localScale;
            ls.x = -1f;
            transform.localScale = ls;
        }
    }

    private void ProcessWallJump()
    {
        if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
        else if (isWallJumping)
        {
            isWallJumping = false;
        }
    }

   
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }
}
