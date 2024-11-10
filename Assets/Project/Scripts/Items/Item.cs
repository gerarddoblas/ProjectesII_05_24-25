using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
   public  GameObject creator;
    public bool consumible = false;
    public virtual void Effect(GameObject target) { }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject != creator)
            Effect(collision.gameObject);
    }

}
