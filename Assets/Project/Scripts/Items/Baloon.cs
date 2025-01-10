using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Baloon : Item
{
    public float jumpIncrease = 2;
    public float duration = 5;
    override public IEnumerator Effect(GameObject target)
    {
        target.GetComponent<Player>().jumpForce += jumpIncrease;
        yield return new WaitForSeconds(duration);
        target.GetComponent<Player>().jumpForce -= jumpIncrease;
        yield return null;
    }
}
