using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerConnectorManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;

    private Transform player1;
    private Transform player2;
    private int playerCount = 0;

    void Start()
    {
        StartCoroutine(WaitForInputManager());
    }

    IEnumerator WaitForInputManager()
    {
        yield return null;

        playerInputManager = PlayerInputManager.instance;

        if (playerInputManager == null)
        {
            Debug.LogError("❌ PlayerInputManager not found in scene.");
            yield break;
        }

        Debug.Log("✅ PlayerInputManager found.");
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    void OnDisable()
    {
        if (playerInputManager != null)
        {
            playerInputManager.onPlayerJoined -= OnPlayerJoined;
        }
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        playerCount++;
        Debug.Log($"🎮 Player {playerCount} joined: {playerInput.transform.name}");

        if (playerCount == 1)
        {
            player1 = playerInput.transform;
        }
        else if (playerCount == 2)
        {
            player2 = playerInput.transform;
            Debug.Log("✅ Both players are now in the scene. Attempting rope connection...");

            if (player1 == null)
            {
                Debug.LogError("❌ player1 is null");
                return;
            }

            var joint = player1.GetComponent<DistanceJoint2D>();
            var rb2 = player2.GetComponent<Rigidbody2D>();

            if (joint == null)
            {
                Debug.LogError($"❌ DistanceJoint2D not found on {player1.name}");
                return;
            }

            if (rb2 == null)
            {
                Debug.LogError($"❌ Rigidbody2D not found on {player2.name}");
                return;
            }

            Debug.Log("🧪 Setting joint.connectedBody and enabling...");
            joint.connectedBody = rb2;
            joint.autoConfigureDistance = true;
            joint.enableCollision = false;
            joint.enabled = true;

            Debug.Log($"🪢 DistanceJoint2D enabled. Connected to {joint.connectedBody.name}. Enabled = {joint.enabled}");
        }
    }
}
