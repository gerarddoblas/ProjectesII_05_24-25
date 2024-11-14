using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Egg : Item { 
    override public void Effect(GameObject target)
    {
        /*if(target.TryGetComponent<Tilemap>(out Tilemap tm))
                tm.RefreshTile(new Vector3Int((int)Mathf.Floor(target.transform.position.x - 0.6f), (int)Mathf.Floor(target.transform.position.y - 0.6f), 0));
        */
        if (target.TryGetComponent<Player>(out Player p))
        {
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, target.GetComponent<Rigidbody2D>().velocity.y);
            p.healthBehaviour.Damage(1);
            Destroy(this.gameObject);
        }
        
     }
}
