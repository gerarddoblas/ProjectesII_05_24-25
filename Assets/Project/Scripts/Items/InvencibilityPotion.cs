using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvencibilityPotion : Item
{
    
    [SerializeField]
    float invencibilityTime = 5;
    override public IEnumerator Effect(GameObject target)
    {
        target.GetComponent<HealthBehaviour>().SetInvincibility(invencibilityTime);
        yield return null;
    }
}
