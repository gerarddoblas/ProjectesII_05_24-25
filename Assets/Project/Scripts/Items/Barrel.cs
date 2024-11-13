using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Item
{
    public float timeInScene = 1.5f;
    public float contador = 0.0f;

    public void Update()
    {
        contador += Time.deltaTime;
        if ( contador >= timeInScene )
        {
            Destroy(this.gameObject);
        }
    }
    override public void Effect(GameObject target)
    {

     if (target.TryGetComponent<Player>(out Player p))
        {
            target.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, target.GetComponent<Rigidbody2D>().velocity.y);
            target.GetComponent<HealthBehaviour>().Damage(2);
            Destroy(this.gameObject);
        }
    }
}
