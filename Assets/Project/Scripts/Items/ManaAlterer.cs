using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaAlterer : Item
{
    [SerializeField]
    float duration = 5f;
    [SerializeField]
    bool fillMaxMana;
    override public IEnumerator Effect(GameObject target)
    {
        if (target.TryGetComponent<Items>(out Items items))
        {
            while (duration > 0)
            {
                items.mana = fillMaxMana ? items.maxFill : 0;
                items.onAlterMana.Invoke(items.mana);
                duration -= Time.deltaTime;
                yield return null;
            }
        }
        Destroy(this);
    }
}
