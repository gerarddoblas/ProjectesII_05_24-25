using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float coyoteTime = 0.5f;
    [SerializeField]private bool grounded;
    [SerializeField] private bool coyote;
    [SerializeField] private AudioClip PlayerInLand;
    public bool Grounded { get { return grounded; } set { grounded = value; } }
    public bool Coyote { get { return coyote; } set { coyote = value; } }

    public enum Landed
    {
        FALLING,
        JUST_LANDED,
        LANDED
    }

    private Landed wasLastGrounded;
    public Landed WasLastGrounded { get { return wasLastGrounded; } }

    private void Start()
    {
        wasLastGrounded = Landed.FALLING;
    }

    private void Update()
    {
        if (grounded && wasLastGrounded == Landed.JUST_LANDED) wasLastGrounded = Landed.LANDED;
        if (grounded && wasLastGrounded == Landed.FALLING) wasLastGrounded = Landed.JUST_LANDED;
        if (!grounded) wasLastGrounded = Landed.FALLING;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameObject.activeInHierarchy)
        {
            grounded = true;
            coyote = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.activeInHierarchy)
        {
            grounded = false;
            StartCoroutine(CoyoteTime());
        }
    }

    IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(coyoteTime);
        coyote = false;
    }
}
