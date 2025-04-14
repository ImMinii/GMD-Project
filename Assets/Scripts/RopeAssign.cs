using UnityEngine;
using UnityEngine.InputSystem;

public class RopeAssign : MonoBehaviour
{
    public RopeConnect ropeConnect; // Reference to the visual rope script

    private Transform player1Transform;
    private Rigidbody2D player1Rb;
    private DistanceJoint2D player1Joint;

    private int playerCount = 0;

    void OnEnable()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    void OnDisable()
    {
        PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        playerCount++;

        if (playerCount == 1)
        {
            player1Transform = playerInput.transform;
            player1Rb = player1Transform.GetComponent<Rigidbody2D>();
            player1Joint = player1Transform.GetComponent<DistanceJoint2D>();

            if (player1Joint != null)
            {
                player1Joint.enabled = false; // Ensure it's off until player 2 connects
            }

            ropeConnect.player1 = player1Transform;
        }
        else if (playerCount == 2)
        {
            Transform player2Transform = playerInput.transform;
            Rigidbody2D player2Rb = player2Transform.GetComponent<Rigidbody2D>();

            ropeConnect.player2 = player2Transform;

            // Now connect the rope (physics)
            if (player1Joint != null && player2Rb != null)
            {
                player1Joint.connectedBody = player2Rb;
                player1Joint.autoConfigureDistance = false;
                player1Joint.distance = 5f; // Adjust this to your rope length
                player1Joint.enableCollision = true;
                player1Joint.enabled = true;

                Debug.Log("Rope connected from Player1 to Player2.");
            }
            else
            {
                Debug.LogWarning("Joint or Rigidbody2D missing — rope can't connect.");
            }
        }
    }
}