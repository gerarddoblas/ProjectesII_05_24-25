using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mina : Item
{
    override public IEnumerator Effect(GameObject target)
    {
        Debug.Log("Collied/Triggered");
        if (target.TryGetComponent<Player>(out Player p))
        {
            Debug.Log("Mine collied with player");
            p.healthBehaviour.Damage(2);
            Destroy(this.gameObject);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Debug.Log("Mine collied with other object");
        }
        yield return null;
    }
}
