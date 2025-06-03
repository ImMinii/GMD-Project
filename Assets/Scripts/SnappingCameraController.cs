using UnityEngine;

public class SnappingCameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player1;
    [SerializeField]
    private Transform player2;
    [SerializeField]
    private Vector2 minBounds;
    [SerializeField]
    private Vector2 maxBounds;

    private float camWidth;
    private float camHeight;

    void Start()
    {
        Camera cam = Camera.main;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
    }

    void Update()
    {
        Vector2 camMin = new Vector2(transform.position.x - camWidth / 2f, transform.position.y - camHeight / 2f);
        Vector2 camMax = new Vector2(transform.position.x + camWidth / 2f, transform.position.y + camHeight / 2f);

        bool p1OutLeft = player1.position.x < camMin.x;
        bool p1OutRight = player1.position.x > camMax.x;
        bool p1OutUp = player1.position.y > camMax.y;
        bool p1OutDown = player1.position.y < camMin.y;

        bool p2OutLeft = player2.position.x < camMin.x;
        bool p2OutRight = player2.position.x > camMax.x;
        bool p2OutUp = player2.position.y > camMax.y;
        bool p2OutDown = player2.position.y < camMin.y;

        Vector3 newPosition = transform.position;

        if (p1OutRight && p2OutRight)
            newPosition += new Vector3(camWidth, 0, 0);
        else if (p1OutLeft && p2OutLeft)
            newPosition += new Vector3(-camWidth, 0, 0);
        else if (p1OutUp && p2OutUp)
            newPosition += new Vector3(0, camHeight, 0);
        else if (p1OutDown && p2OutDown)
            newPosition += new Vector3(0, -camHeight, 0);
        
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        if (newPosition != transform.position)
            transform.position = newPosition;
    }
}