using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float speed = 4f;

    private float horizontalMovement;
    
    private void Start()
    {
        //body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        body.linearVelocity = new Vector2(horizontalMovement * speed, body.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }
}
