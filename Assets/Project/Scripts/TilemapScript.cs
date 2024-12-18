using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapScript : MonoBehaviour
{
    [SerializeField] private Tilemap tm;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.GetComponent<Item>() == null) return;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            tm.RefreshTile(new Vector3Int((int)Mathf.Floor(contact.point.x), (int)Mathf.Ceil(contact.point.y) + 1, 0));
            tm.RefreshTile(new Vector3Int((int)Mathf.Ceil(contact.point.x), (int)Mathf.Floor(contact.point.y), 0));
            tm.RefreshTile(new Vector3Int((int)Mathf.Ceil(contact.point.x), (int)Mathf.Floor(contact.point.y) + 1, 0));
        }
        source.Play();
        //TODO: Check edge cases
    }
}
