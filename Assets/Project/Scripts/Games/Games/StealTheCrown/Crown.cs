using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crown : MonoBehaviour
{
    GameObject owner;
    
    public GameObject GetOwner() { return owner; }
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (owner == null)
        {
            setOwner(collision.gameObject);
            boxCollider.enabled = false;
        }
    }
    private void Update()
    {
        if (owner != null)
            transform.position = owner.transform.position + Vector3.up;           
    }
    public void LooseCrown()
    {
        owner = null;
    }
    public void setOwner(GameObject newOwner) { 
        owner = newOwner;
    }
}
