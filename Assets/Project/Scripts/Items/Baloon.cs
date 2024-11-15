using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Baloon : Item
{
    
    override public void Effect(GameObject target)
    {
        target.GetComponent<Player>().jumpForce += .5f;
        if (target.GetComponent<Player>().jumpForce > 10)
            target.GetComponent<Player>().jumpForce = 10;
    }
}
