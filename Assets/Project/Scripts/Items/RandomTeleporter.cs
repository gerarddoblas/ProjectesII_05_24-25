using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTeleporter : Item
{
    override public IEnumerator Effect(GameObject target)
    {
       // LeanTween.scale(target.gameObject, Vector3.zero, .5f).setOnComplete(delegate ()
       // {
            target.transform.position = AreaManager.Instance.GetRandomAreaPosition();
            //target.transform.localScale = Vector3.one*4;
        //}).setEaseInOutBounce();
        yield return null;
    }
}
