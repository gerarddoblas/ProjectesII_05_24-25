using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Item
{
    public float timeInScene = 0.5f;
    public float contador = 0.0f;
    /*
    contador += Time.deltaTime;
        if (contador == timeInScene)
        {
            Destroy(this.gameObject);
        }*/
    override public void Effect(GameObject target)
    {

     if (target.TryGetComponent<Player>(out Player p))
        {
            target.GetComponent<HealthBehaviour>().Damage(2);
            Destroy(this.gameObject);
        }
    }
}
