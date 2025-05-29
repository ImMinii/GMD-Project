using UnityEngine;
using UnityEngine.InputSystem;

public class ManualInputAssignment : MonoBehaviour
{
    public PlayerInput player1Input;
    public PlayerInput player2Input; 

    void Start()
    {
        var gamepads = Gamepad.all;

        // Assign Gamepad 1 to Player 1, Gamepad 2 to Player 2
        if (gamepads.Count >= 2)
        {
            player1Input.SwitchCurrentControlScheme("Gamepad", gamepads[0]);
            player2Input.SwitchCurrentControlScheme("Gamepad", gamepads[1]);
        }
        else
        {
            Debug.LogWarning("Not enough controllers found.");
        }
    }
}