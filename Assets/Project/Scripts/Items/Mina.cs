using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mina : Item
{

    override public void Effect(GameObject target)
    {


        if (target.TryGetComponent<Player>(out Player p))
        {
            target.GetComponent<HealthBehaviour>().Damage(2);
            Destroy(this.gameObject);
        }
        else
        {
            if (target.gameObject)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }

}
