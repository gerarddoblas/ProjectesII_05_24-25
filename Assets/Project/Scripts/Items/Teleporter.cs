using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Teleporter : Item
{
    override public IEnumerator Effect(GameObject target)
    {
        if (target.TryGetComponent<Player>(out Player p))
        {
            Vector3 creatorPos = creator.transform.position; 
            creator.transform.position = target.transform.position;
            target.transform.position = creatorPos;
        }
        else
            creator.transform.position = this.transform.position;
        Destroy(this.gameObject);
        yield return null;
    }
}