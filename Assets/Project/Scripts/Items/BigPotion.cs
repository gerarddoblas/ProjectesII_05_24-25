using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPotion : Item
{
    public float incrementMult = 1.5f;
    public float duration = 5;
    override public IEnumerator Effect(GameObject target)
    {
        while (target.transform.localScale != new Vector3(4, 4, 4))
            yield return null;
        LeanTween.scale(target, target.transform.localScale * incrementMult, 1).setEaseInOutBounce();
        yield return new WaitForSeconds(duration);
        LeanTween.scale(target, target.transform.localScale / incrementMult, 1).setEaseInOutBounce();
        yield return null;
    }
}
