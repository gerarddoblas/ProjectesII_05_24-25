using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
   public  GameObject creator;
    public bool consumible = false;
    public bool creatorImmunity = false;
    public virtual void Effect(GameObject target) { }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject != creator && !creatorImmunity)
            Effect(collision.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != creator && !creatorImmunity)
            Effect(collision.gameObject);
    }
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
