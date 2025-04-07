using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInstantiation : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    void Start()
    {
        var player1 = PlayerInput.Instantiate(playerPrefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[0]);
        var player2 = PlayerInput.Instantiate(playerPrefab, controlScheme: "Keyboard&Mouse", pairWithDevice: Keyboard.current);
    }
}
