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
    private List<Rigidbody2D> playersOnPlatform = new List<Rigidbody2D>();

    void Start()
    {
        target = pointB.position;
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
        Vector3 movementDelta = newPosition - transform.position;
        transform.position = newPosition;

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }

        // Move all players on platform manually
        foreach (var rb in playersOnPlatform)
        {
            if (rb != null)
                rb.MovePosition(rb.position + (Vector2)movementDelta);
        }

        previousPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInLayerMask(collision.gameObject, stickableLayers))
        {
            Rigidbody2D rb = collision.rigidbody;
            if (rb != null && !playersOnPlatform.Contains(rb))
                playersOnPlatform.Add(rb);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsInLayerMask(collision.gameObject, stickableLayers))
        {
            Rigidbody2D rb = collision.rigidbody;
            if (rb != null)
                playersOnPlatform.Remove(rb);
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((1 << obj.layer) & mask) != 0;
    }
}