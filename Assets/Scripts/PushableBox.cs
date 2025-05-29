using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PushableBox : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        FreezeXMovement();
    }
    
    private void FixedUpdate()
    {
        // If the box is moving up (positive Y), clamp the velocity to zero (no upward movement)
        if (rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
    }


    public void FreezeXMovement()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
    }

    public void AllowXMovement()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Only rotation is frozen
    }
}