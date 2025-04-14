using UnityEngine;
using System.Collections.Generic;

public class RopeConnect : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public GameObject ropeSegmentPrefab;
    public int segmentCount = 10;

    private List<GameObject> ropeSegments = new List<GameObject>();

    void Start()
    {
        if (player1 != null && player2 != null)
        {
            GenerateRope();
        }
    }

    void GenerateRope()
    {
        Vector2 startPos = player1.position;
        Vector2 endPos = player2.position;
        Vector2 direction = (endPos - startPos).normalized;
        float totalLength = Vector2.Distance(startPos, endPos);
        float segmentLength = totalLength / segmentCount;

        GameObject previous = null;

        for (int i = 0; i < segmentCount; i++)
        {
            Vector2 segmentPos = startPos + direction * segmentLength * i;
            GameObject segment = Instantiate(ropeSegmentPrefab, segmentPos, Quaternion.identity, transform);
            ropeSegments.Add(segment);

            if (i == 0)
            {
                // Connect first segment to player1
                HingeJoint2D joint = segment.GetComponent<HingeJoint2D>();
                joint.connectedBody = player1.GetComponent<Rigidbody2D>();
            }
            else
            {
                // Connect to previous rope segment
                HingeJoint2D joint = segment.GetComponent<HingeJoint2D>();
                joint.connectedBody = previous.GetComponent<Rigidbody2D>();
            }

            previous = segment;
        }

        // Last segment connects to player2
        HingeJoint2D lastJoint = ropeSegments[segmentCount - 1].GetComponent<HingeJoint2D>();
        lastJoint.connectedBody = player2.GetComponent<Rigidbody2D>();
    }
}