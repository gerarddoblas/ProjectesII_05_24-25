using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestPhysics : MonoBehaviour
{
    [SerializeField] private Tilemap tm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        tm = collision.gameObject.GetComponent<Tilemap>();
        if (tm != null)
        {
            tm.RefreshTile(new Vector3Int((int)Mathf.Ceil(transform.position.x) - 1, (int)Mathf.Ceil(transform.position.y) - 1, 0));
            Debug.Log(new Vector3Int((int)Mathf.Ceil(transform.position.x) - 1, (int)Mathf.Ceil(transform.position.x) - 1, 0));
        }
    }
}
