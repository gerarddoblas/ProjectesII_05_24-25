using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumpGraceTime = 0.5f;
    private float jumpTime = 0.0f;
    [SerializeField] private float score;
    public float Score { set { score = value; } get { return score; } }
    public float acceleration ,maxSpeed, jumpForce;
    public float maxJumpForce, minJumpForce, curJumpForce, deltaJumpForce;
    public bool jumping = false;
    public Vector2 playerSpeed = Vector2.zero;
    public UnityEvent<float> onAlterMana;
    public UnityEvent<float> onAlterScore;
    public PlayerInput input;
    public Rigidbody2D rigidbody2D;
    public GroundCheck groundCheck;
    public SpriteRenderer spriteRenderer;
    public HealthBehaviour healthBehaviour;
    Animator animator;

    //Knockout vars
    private bool canMove = true;
    public float knowdownTime = 3f;
    private AudioSource source;
    [SerializeField] private AudioClip jumpClip, KnockoutClip;
    public SpriteRenderer positionMarker;
    public void LockMovement() { canMove = false; }
    public void UnlockMovement() {  canMove = true; }
    void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        input = GetComponent<PlayerInput>();
        input.actions.FindAction("Move").performed += Move;
        input.actions.FindAction("Move").canceled += MoveCancelled;
        input.actions.FindAction("Jump").started += Jump;
        input.actions.FindAction("Jump").canceled += JumpStop;
        healthBehaviour = GetComponent<HealthBehaviour>();
        healthBehaviour.OnDie.AddListener(delegate ()
        {
            StartCoroutine(this.Knockout());
        });
    }
    private void FixedUpdate()
    { 

        rigidbody2D.AddForce(new Vector2(acceleration*playerSpeed.x*Time.deltaTime,0), ForceMode2D.Force);
        if (rigidbody2D.velocity.x > maxSpeed)
            rigidbody2D.velocity = new Vector2(maxSpeed, rigidbody2D.velocity.y);
        else if (rigidbody2D.velocity.x < -maxSpeed)
            rigidbody2D.velocity = new Vector2(-maxSpeed, rigidbody2D.velocity.y);
       
    }
    private void Update()
    {
        UpdateAnimations();
        if(jumpTime > 0) TryJump();
        if (input.actions.FindAction("Jump").IsPressed()) JumpHold();
    }
    
    void UpdateAnimations()
    {
        animator.SetBool("canMove", canMove);
        animator.SetBool("invencibility", healthBehaviour.invencibility);
        animator.SetBool("isGrounded",groundCheck.Grounded);
        animator.SetFloat("horizontalSpeed",rigidbody2D.velocity.x);
        animator.SetFloat("verticalSpeed", rigidbody2D.velocity.y);
    }
    void Move(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            playerSpeed = context.ReadValue<Vector2>();
            if (playerSpeed.x > 0)
                spriteRenderer.flipX = true;
            else if (playerSpeed.x < 0)
                spriteRenderer.flipX = false;
        }
    }
    void MoveCancelled(InputAction.CallbackContext context)
    {
        playerSpeed = Vector2.zero;
    }
    void Jump(InputAction.CallbackContext context)
    {
        jumpTime = jumpGraceTime;
        TryJump();
    }

    void TryJump()
    {
        jumpTime -= Time.deltaTime;
        if (!(groundCheck.Grounded && canMove)) return;

        jumping = true;

        rigidbody2D.AddForce(new Vector2(0, minJumpForce), ForceMode2D.Impulse);
        curJumpForce = minJumpForce;
        groundCheck.Grounded = false;
        source.clip = jumpClip;
        source.Play();
        jumpTime = 0;
    }

    void JumpHold()
    {
        if(jumping && curJumpForce < maxJumpForce)
        {
            rigidbody2D.AddForce(new Vector2(0, deltaJumpForce * Time.deltaTime), ForceMode2D.Impulse);
            curJumpForce += deltaJumpForce * Time.deltaTime;
        }
    }

    void JumpStop(InputAction.CallbackContext context) => jumping = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player p))
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(rigidbody2D.velocity.magnitude, 0),ForceMode2D.Impulse) ;
    }
    IEnumerator Knockout()
    {
        canMove = false;
        source.clip = KnockoutClip;
        source.Play(); 
        StartCoroutine(healthBehaviour.SetInvencibility(knowdownTime*2));
        yield return new WaitForSeconds(knowdownTime);
        canMove = true;
        healthBehaviour.FullHeal();
    }
}