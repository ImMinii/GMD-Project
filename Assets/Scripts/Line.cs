using UnityEngine;

public class Line : MonoBehaviour
{
    public Transform objectA; // Assign Player 1
    public Transform objectB; // Assign Player 2
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Line needs two points
    }

    void FixedUpdate()
    {
        if (objectA != null && objectB != null)
        {
            lineRenderer.SetPosition(0, objectA.position);
            lineRenderer.SetPosition(1, objectB.position);
        }
    }
}
