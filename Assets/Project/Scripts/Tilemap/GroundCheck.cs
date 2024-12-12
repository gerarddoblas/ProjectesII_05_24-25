using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float coyoteTime = 0.5f;
    [SerializeField]private bool grounded;
    public bool Grounded { get { return grounded; } set { grounded = value; } }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameObject.activeInHierarchy)
            grounded = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(gameObject.activeInHierarchy)
            StartCoroutine(CoyoteTime());
    }

    IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(coyoteTime);
        grounded = false;
    }
}
