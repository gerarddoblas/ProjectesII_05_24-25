using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapScript : MonoBehaviour
{
    [SerializeField] private Tilemap tm;
    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<Tilemap>();
        for (int i = -2; i < 2; i++)
        {
            ExplodingTile et = (ExplodingTile)tm.GetTile(new Vector3Int(i, -1, 0));
            if (et != null && et.started)
            {
                et.started = false;
                Debug.Log("refreshing at " + new Vector3Int(i, -1, 0));
                tm.RefreshTile(new Vector3Int(i, -1, 0));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        tm.RefreshTile(new Vector3Int((int)Mathf.Floor(collision.transform.position.x - 0.6f), (int)Mathf.Floor(collision.transform.position.y - 0.6f), 0));
        Debug.Log(new Vector3Int((int)Mathf.Floor(collision.transform.position.x - 0.6f), (int)Mathf.Floor(collision.transform.position.y - 0.6f), 0));
    }
}
