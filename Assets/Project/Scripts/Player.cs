using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ItemsCreation : MonoBehaviour
{


    public float speed = 1f, jumpForce = 10f;
    public Vector2 playerSpeed = Vector2.zero;

    Rigidbody2D rigidbody2D;
    GroundCheck groundCheck;

    public SpriteRenderer positionMarker;
    void Awake()
    {

        rigidbody2D = GetComponent<Rigidbody2D>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        PlayerInput input = GetComponent<PlayerInput>();
        input.actions.FindAction("Move").performed += Move;
        input.actions.FindAction("Move").canceled += MoveCancelled;
        input.actions.FindAction("Jump").started += Jump;

    }
    private void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(playerSpeed.x * speed * Time.deltaTime, rigidbody2D.velocity.y);
    }
    private void Update()
    {

    }
    void Move(InputAction.CallbackContext context)
    {
        playerSpeed = context.ReadValue<Vector2>();
    }
    void MoveCancelled(InputAction.CallbackContext context)
    {
        playerSpeed = Vector2.zero;
    }
    void Jump(InputAction.CallbackContext context)
    {
        if (groundCheck.grounded)
            rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }


}