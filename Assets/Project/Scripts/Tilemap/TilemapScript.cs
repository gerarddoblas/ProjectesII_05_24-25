using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapScript : MonoBehaviour
{
    [SerializeField] private Tilemap tm;
    private AudioSource source;
    [SerializeField] private float tolerance;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Item>() == null) return;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            tm.RefreshTile(tm.layoutGrid.WorldToCell(contact.point));
            tm.RefreshTile(tm.layoutGrid.WorldToCell(contact.point + Vector2.up * tolerance));
            tm.RefreshTile(tm.layoutGrid.WorldToCell(contact.point + Vector2.down * tolerance));
            tm.RefreshTile(tm.layoutGrid.WorldToCell(contact.point + Vector2.left * tolerance));
            tm.RefreshTile(tm.layoutGrid.WorldToCell(contact.point + Vector2.right * tolerance));
        }
        source.Play();
    }
}
