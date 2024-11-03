using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float mana, fillspeed, maxFill;
    public float consumedMana;
    public float speed = 1f, jumpForce = 10f;
    public Vector2 playerSpeed = Vector2.zero;
    public UnityEvent<float> onAlterMana;
    Rigidbody2D rigidbody2D;
    GroundCheck groundCheck;
    Coroutine charging;
    public SpriteRenderer positionMarker;
    void Awake()
    {
        mana = 0;
        fillspeed = 0.1f;
        maxFill = 3;
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        PlayerInput input = GetComponent<PlayerInput>();
        input.actions.FindAction("Move").performed += Move;
        input.actions.FindAction("Move").canceled += MoveCancelled;
        input.actions.FindAction("Jump").started += Jump;
        input.actions.FindAction("Attack").started += StartCreating;
        input.actions.FindAction("Attack").canceled += Create;
    }
    private void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(playerSpeed.x * speed * Time.deltaTime, rigidbody2D.velocity.y);
    }
    private void Update()
    {
        if (mana < maxFill)
        {
            mana += fillspeed * Time.deltaTime;
            if (mana >= maxFill)
                mana = maxFill;
            onAlterMana.Invoke(this.mana);
        }
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
        if(groundCheck.grounded)
            rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
     }
    void StartCreating(InputAction.CallbackContext context) 
    {
        if(mana >= 1)
            consumedMana = 1;
        charging = StartCoroutine(CharginCreation(context));
    }
    IEnumerator CharginCreation(InputAction.CallbackContext context)
    {
        while (true)
        {
            Debug.Log("Consuming Mana");
            if (consumedMana < mana)
            {
                consumedMana += (fillspeed * Time.deltaTime * 4);
                if (consumedMana > mana)
                    consumedMana = mana;
            }
            yield return null;
        }
    }
    void Create(InputAction.CallbackContext context) {
        StopCoroutine(charging);
        charging = null;
        Debug.Log("Creating...");
        switch ((int)consumedMana)
        {
            case 1:
                Debug.Log("generating small object");
                break;
            case 2:
                Debug.Log("generating mid object");
                break;
            case 3:
                Debug.Log("generating big object");
                break;
        }
        mana -=(int)consumedMana;
        onAlterMana.Invoke(this.mana);
        consumedMana = 0;
    }
}
