using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]private PlayerInput input;
    [SerializeField]private Rigidbody2D rigidbody2D;
    [SerializeField]private GroundCheck groundCheck;
    [SerializeField]private SpriteRenderer spriteRenderer;
    [SerializeField]public HealthBehaviour healthBehaviour;
    [SerializeField]private Animator animator;

    [Header("Push")]
    [SerializeField] private GameObject pushLeft;
    [SerializeField] private GameObject pushRight;

    [Header("Physics Attributes")]
    public float acceleration, maxSpeed, jumpForce;
    [SerializeField] private float maxJumpForce, minJumpForce, curJumpForce, deltaJumpForce;
    [SerializeField] private float jumpGraceTime = 0.5f;
    private float jumpTime = 0.0f;
    public bool jumping = false;
    public Vector2 playerSpeed = Vector2.zero;

    [Header("Knockout Variables")]
    public bool canMove = true;
    public float knockoutTime = 3f;

    [Header("Events")]
    public UnityEvent<float> onAlterMana;
    public UnityEvent<float> onAlterScore;
    public UnityEvent<GameObject> onPlayerHitted;

    [Header("Score")]
    [SerializeField] private float score;
    public float Score { set { score = value; } get { return score; } }

    [Header("VFX")]
    [SerializeField] private GameObject landingParticles;
    [SerializeField] private GameObject jumpParticles;

    [Header("SFX")]
    private AudioSource source;
    [SerializeField] private AudioClip KnockoutClip;
    public SpriteRenderer positionMarker;
    public void LockMovement() { canMove = false; }
    public void UnlockMovement() {  canMove = true; }
    void Awake()
    { 
        input = GetComponent<PlayerInput>();

        input.actions.FindAction("Move").performed += Move;
        input.actions.FindAction("Move").canceled += MoveCancelled;
        input.actions.FindAction("Jump").started += Jump;
        input.actions.FindAction("Jump").canceled += JumpStop;
        input.actions.FindAction("GenerateSmallObject").started += Push;
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundCheck = GetComponentInChildren<GroundCheck>();
       

        healthBehaviour = GetComponent<HealthBehaviour>();
        healthBehaviour.OnDie.AddListener(delegate ()
        {
            StartCoroutine(this.Knockout());
        });
    }
    private void FixedUpdate()
    {

        rigidbody2D.AddForce(new Vector2(acceleration * playerSpeed.x * Time.deltaTime, 0), ForceMode2D.Force);
        rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -maxSpeed, maxSpeed), rigidbody2D.velocity.y);

    }
    private void Update()
    {
        UpdateAnimations();
        if (groundCheck.WasLastGrounded == GroundCheck.Landed.JUST_LANDED)
            Instantiate(landingParticles, groundCheck.transform.position, Quaternion.identity);
        if (jumpTime > 0) TryJump();
        if (input.actions.FindAction("Jump").IsPressed()) JumpHold();
    }
    
    void UpdateAnimations()
    {
        animator.SetBool("canMove", canMove);
        animator.SetBool("invencibility", healthBehaviour.invincibility);
        animator.SetBool("isGrounded",groundCheck.Grounded);
        animator.SetFloat("horizontalSpeed", rigidbody2D.velocity.x);
        animator.SetBool("MovingHorizontally",(playerSpeed.x != 0));
        animator.SetFloat("verticalSpeed", rigidbody2D.velocity.y);
    }
    void Move(InputAction.CallbackContext context)
    {
        /*if (!canMove) return;

        playerSpeed = context.ReadValue<Vector2>();
        if (playerSpeed.x == 0) return;
        spriteRenderer.flipX = playerSpeed.x < 0;*/
        if (canMove)
        {
            source.Play();
            playerSpeed = context.ReadValue<Vector2>();
            if (playerSpeed.x < 0)
                spriteRenderer.flipX = true;
            else if (playerSpeed.x > 0)
                spriteRenderer.flipX = false;
        }
    }
    void MoveCancelled(InputAction.CallbackContext context)
    {
        playerSpeed = Vector2.zero;
        source.Stop();
    }
    void Jump(InputAction.CallbackContext context)
    {
        jumpTime = jumpGraceTime;
        TryJump();
    }

    void TryJump()
    {
        jumpTime -= Time.deltaTime;
        if (!(groundCheck.Coyote && canMove)) return;

        jumping = true;
        jumpTime = 0;

        rigidbody2D.AddForce(new Vector2(0, minJumpForce), ForceMode2D.Impulse);
        curJumpForce = minJumpForce;
        groundCheck.Grounded = false;
        groundCheck.Coyote = false;

        AudioManager.instance.PlaySFX("Jump");
       // AudioManager.instance.SetSFXVolume(0.2f);
        jumpTime = 0;
        Instantiate(jumpParticles, groundCheck.transform.position, Quaternion.identity);
    }

    void JumpHold()
    {
        if(jumping && curJumpForce < maxJumpForce && rigidbody2D.velocity.y > 0)
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
        AudioManager.instance.PlaySFX("Knockout");
        this.GetComponent<Items>().LockObjectCreation();
        StartCoroutine(healthBehaviour.SetInvincibility(knockoutTime * 2));
        yield return new WaitForSeconds(knockoutTime);
        this.GetComponent<Items>().UnlockObjectCreation();
        canMove = true;
        healthBehaviour.FullHeal();
    }
    IEnumerator SetSnockout()
    {
        canMove = false;
        healthBehaviour.SetInvincibility();
        yield return null;
    }
    IEnumerator QuitKnockout() {
        canMove = true;
        healthBehaviour.QuitInvincibility();
        yield return null;
    }
    public bool GetKO() { return (!canMove && healthBehaviour.invincibility); }

    private void Push(InputAction.CallbackContext context)
    {
        StartCoroutine(this.ActivatePush((spriteRenderer.flipX) ? pushLeft : pushRight));
    }

    IEnumerator ActivatePush(GameObject push)
    {
        if (canMove)
        {
            push.SetActive(true);
            PushScript pushScript = push.GetComponent<PushScript>();
            yield return new WaitForSeconds(0.1f);
            push.SetActive(false);
        }
        yield return null;
    }
    private void OnDestroy()
    {
        input.actions.FindAction("Move").RemoveAllBindingOverrides();
        input.actions.FindAction("Jump").RemoveAllBindingOverrides();
        input.actions.FindAction("GenerateSmallObject").RemoveAllBindingOverrides();
    }
}