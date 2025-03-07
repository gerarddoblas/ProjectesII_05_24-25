using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingBoots : Item
{
    public float incrementMult = 1.5f;
    public float duration = 5;
    override public IEnumerator Effect(GameObject target)
    {
        Player p = target.GetComponent<Player>();
        while (p.maxSpeed!=13)
            yield return null;

        LeanTween.value(13,13*incrementMult,1).setEaseInOutElastic().setOnUpdate((float r) => { 
            p.maxSpeed = r;
        }).setOnStart(delegate () {
            LeanTween.value(1, 2, 1).setOnUpdate((float r) =>
            {
                p.GetComponent<Animator>().speed = r;
            });
        });
        yield return new WaitForSeconds(duration);
        LeanTween.value(p.maxSpeed, 13, 1).setEaseInOutElastic().setOnUpdate((float r) => {
            p.maxSpeed = r; 
        }).setOnStart(delegate () {
            LeanTween.value(2, 1, 1).setOnUpdate((float r) =>
            {
                p.GetComponent<Animator>().speed = r;
            });
        });
        yield return null;
    }
}
