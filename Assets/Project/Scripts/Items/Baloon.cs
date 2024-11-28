using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Baloon : Item
{
    
    override public void Effect(GameObject target)
    {
        target.GetComponent<Player>().jumpForce += 1.5f;
        if (target.GetComponent<Player>().jumpForce > 35)
            target.GetComponent<Player>().jumpForce = 35;
    }
}
