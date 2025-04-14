using UnityEngine;
using UnityEngine.InputSystem;

public class ManualInputAssignment : MonoBehaviour
{
    public PlayerInput player1;
    public PlayerInput player2;

    void Start()
    {
        var devices = InputSystem.devices;

        // Find the first gamepad and assign to player1
        Gamepad gamepad = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
        Keyboard keyboard = Keyboard.current;

        if (player1 != null && keyboard != null)
        {
            player1.SwitchCurrentControlScheme("Keyboard", keyboard);
        }

        if (player2 != null && gamepad != null)
        {
            player2.SwitchCurrentControlScheme("Gamepad", gamepad);
        }
    }
}