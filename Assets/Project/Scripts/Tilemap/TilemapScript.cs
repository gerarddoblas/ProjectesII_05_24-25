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
            CollideAt(contact.point);
        source.Play();
    }
    
    public void CollideAt(Vector2 pos)
    {
        tm.RefreshTile(tm.layoutGrid.WorldToCell(pos));
        tm.RefreshTile(tm.layoutGrid.WorldToCell(pos + Vector2.up * tolerance));
        tm.RefreshTile(tm.layoutGrid.WorldToCell(pos + Vector2.down * tolerance));
        tm.RefreshTile(tm.layoutGrid.WorldToCell(pos + Vector2.left * tolerance));
        tm.RefreshTile(tm.layoutGrid.WorldToCell(pos + Vector2.right * tolerance));
        tm.RefreshTile(tm.layoutGrid.WorldToCell(pos + Vector2.up * tolerance + Vector2.left * tolerance));
        tm.RefreshTile(tm.layoutGrid.WorldToCell(pos + Vector2.down * tolerance + Vector2.left * tolerance));
        tm.RefreshTile(tm.layoutGrid.WorldToCell(pos + Vector2.up * tolerance + Vector2.right * tolerance));
        tm.RefreshTile(tm.layoutGrid.WorldToCell(pos + Vector2.down * tolerance + Vector2.right * tolerance));
    }

    public void CollideAtArea(Vector2 pos, int radius)
    {
        for (int x = 0; x < radius; ++x)
            for (int y = 0; y < radius; ++y)
                if (x * x + y * y < radius)
                {
                    CollideAt(pos + new Vector2(x, y));
                    CollideAt(pos + new Vector2(-x, y));
                    CollideAt(pos + new Vector2(x, -y));
                    CollideAt(pos + new Vector2(-x, -y));
                }
    }
}
