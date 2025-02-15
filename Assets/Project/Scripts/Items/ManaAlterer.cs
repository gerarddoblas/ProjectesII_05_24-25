using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaAlterer : Item
{
    [SerializeField]
    float itemDuration = 5f;
    [SerializeField]
    bool fillMaxMana;
    override public IEnumerator Effect(GameObject target)
   {
        float duration = itemDuration;
        if (target.TryGetComponent<Items>(out Items items))
        {
            while (duration > 0)
            {
                if (fillMaxMana)
                    items.mana = items.maxFill;
                else
                    items.mana = 0;
                yield return null;
                items.onAlterMana.Invoke(items.mana);
                yield return null;
                duration -= Time.deltaTime;
                yield return null;
            }
        }
        yield return null;
        Destroy(this.gameObject);
    }
}
