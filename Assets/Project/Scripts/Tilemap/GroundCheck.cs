using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float coyoteTime = 0.5f;
    [SerializeField]private bool grounded;
    [SerializeField] private bool coyote;
    public bool Grounded { get { return grounded; } set { grounded = value; } }
    public bool Coyote { get { return coyote; } set { coyote = value; } }
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
