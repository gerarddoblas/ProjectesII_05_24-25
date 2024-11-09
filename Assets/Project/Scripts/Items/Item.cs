using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    GameObject creator;
    public virtual void Effect(Collision2D collision) { }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject != creator)
            Effect(collision);
    }

}
