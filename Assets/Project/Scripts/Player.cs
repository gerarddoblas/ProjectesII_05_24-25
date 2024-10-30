using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float mana, fillspeed, maxFill;
    public float consumedMana;
    public float speed = 1f, jumpForce = 10f;
    public Vector2 playerSpeed = Vector2.zero;
    Rigidbody2D rigidbody2D;
    GroundCheck groundCheck;
    void Start()
    {
        mana = 0;
        fillspeed = 0.1f;
        maxFill = 3;
        //Getting components
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        //Input settings
        PlayerInput input = GetComponent<PlayerInput>();
        //Move
        input.actions.FindAction("Move").performed += Move;
        input.actions.FindAction("Move").canceled += MoveCancelled;
        //Jump
        input.actions.FindAction("Jump").started += Jump;
        //Attack/Create
        input.actions.FindAction("Attack").started += StartCreating;
        input.actions.FindAction("Attack").performed += CharginCreation;
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
        //if(mana >= 1)
        //    consumedMana = 1;
    }
    void CharginCreation(InputAction.CallbackContext context)
    {
        Debug.Log("Hi");
       //if (consumedMana < mana)
        //{
            consumedMana += fillspeed * Time.deltaTime;
            //if (consumedMana > mana)
            //    consumedMana = mana;
        //}
    }
    void Create(InputAction.CallbackContext context) {
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
        consumedMana = 0;
    }
    /*  void InputTest(InputAction.CallbackContext context)
      {
          if (context.performed)
          {
              print("Action performed");
          }
          else if (context.started)
          {
              print("Action started");
          }
          else if (context.canceled)
          {
              print("Action canceled");
          }
      }*/

}
