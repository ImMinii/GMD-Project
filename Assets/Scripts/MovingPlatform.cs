using UnityEngine;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public LayerMask stickableLayers;

    private Vector3 target;
    private Vector3 previousPosition;
    private Vector3 movementDelta;

    private void Start()
    {
        target = pointB.position;
        previousPosition = transform.position;
    }

    public bool isStopped = false;

    private void FixedUpdate()
    {
        if (isStopped) return; // Stop movement if time is stopped

        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
        movementDelta = newPosition - transform.position;
        transform.position = newPosition;

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }

        previousPosition = transform.position;
    }
private void OnCollisionEnter2D(Collision2D collision)
{
    // Check if the object colliding is the player
    if (IsInLayerMask(collision.gameObject, stickableLayers))
    {
        // Only set parent if player's ground check confirms grounded on this platform
        // (Or, for simple tests, just set parent when any collision with stickable layer)
        collision.transform.SetParent(this.transform);
    }
}

private void OnCollisionExit2D(Collision2D collision)
{
    if (IsInLayerMask(collision.gameObject, stickableLayers))
    {
        if (collision.transform.parent == this.transform)
        {
            collision.transform.SetParent(null);
        }
    }
}


    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((1 << obj.layer) & mask) != 0;
    }
}
