using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Egg : Item { 
    public void Effect(Collision2D collision)
    {
       collision.gameObject.GetComponent<Tilemap>().RefreshTile(new Vector3Int((int)Mathf.Floor(collision.transform.position.x - 0.6f), (int)Mathf.Floor(collision.transform.position.y - 0.6f), 0));
    }
}
