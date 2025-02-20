using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crown : MonoBehaviour
{
    GameObject owner;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(owner==null)
            setOwner(collision.gameObject);
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
