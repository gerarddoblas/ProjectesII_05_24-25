using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    [SerializeField]
    bool fullHeal = false;
    [SerializeField]
    int healthAmount = 5;
    [SerializeField] private GameObject particles;
    override public IEnumerator Effect(GameObject target)
    {
        Instantiate(particles, this.transform.position, Quaternion.identity);
        if (fullHeal)
            target.GetComponent<HealthBehaviour>().FullHeal();
        else
            target.GetComponent<HealthBehaviour>().Heal(healthAmount);
        yield return null;
    }
}
