using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float score;
    public float Score { set { score = value; } get { return score; } }
    public float acceleration ,maxSpeed, jumpForce;
    public Vector2 playerSpeed = Vector2.zero;
    public UnityEvent<float> onAlterMana;
    public UnityEvent<float> onAlterScore;
    Rigidbody2D rigidbody2D;
    GroundCheck groundCheck;
    SpriteRenderer spriteRenderer;
    private AudioSource source;
    [SerializeField] private AudioClip jumpClip;

    public SpriteRenderer positionMarker;
    void Awake()
    {
        source = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        PlayerInput input = GetComponent<PlayerInput>();
        input.actions.FindAction("Move").performed += Move;
        input.actions.FindAction("Move").canceled += MoveCancelled;
        input.actions.FindAction("Jump").started += Jump;
    }
    private void FixedUpdate()
    {
        //rigidbody2D.velocity = new Vector2(playerSpeed.x * speed * Time.deltaTime, rigidbody2D.velocity.y);
        rigidbody2D.AddForce(new Vector2(acceleration*playerSpeed.x*Time.deltaTime,0), ForceMode2D.Force);
        if (rigidbody2D.velocity.x > maxSpeed)
            rigidbody2D.velocity = new Vector2(maxSpeed, rigidbody2D.velocity.y);
        else if (rigidbody2D.velocity.x < -maxSpeed)
            rigidbody2D.velocity = new Vector2(-maxSpeed, rigidbody2D.velocity.y);
    }
    private void Update()
    {

    }
    void Move(InputAction.CallbackContext context)
    {
        playerSpeed = context.ReadValue<Vector2>();
        if(playerSpeed.x > 0)
            spriteRenderer.flipX = true;
        else if(playerSpeed.x < 0)
                spriteRenderer.flipX = false;
            
    }
    void MoveCancelled(InputAction.CallbackContext context)
    {
        playerSpeed = Vector2.zero;
    }
    void Jump(InputAction.CallbackContext context)
    {
        if (groundCheck.grounded)
        {
            rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            source.clip = jumpClip;
            source.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player p))
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(rigidbody2D.velocity.magnitude, 0),ForceMode2D.Impulse) ;
    }
}