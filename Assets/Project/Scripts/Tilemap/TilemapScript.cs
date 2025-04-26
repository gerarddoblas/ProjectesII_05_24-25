using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TilemapScript : MonoBehaviour
{
    [SerializeField] float tolerance = 0.3f;
    private Tilemap tm;
    private AudioSource source;
    // Start is called before the first frame update
    public static TilemapScript Instance { get; private set; }
    private void Awake()
    {
        tm = GetComponent<Tilemap>();
        GetComponent<Tilemap>().RefreshAllTiles();
        SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadSceneMode)
        {
            Instance = this;
            tm.RefreshAllTiles();
        };
    }
    void Start()
    {
        source = GetComponent<AudioSource>();
        tm = GetComponent<Tilemap>();
        tm.RefreshAllTiles();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Bounce(collision);

        if (collision.gameObject.GetComponent<Item>() != null)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Dictionary<Vector3Int, ExplodingTile> explodingTiles = CollideAt<ExplodingTile>(contact.point);
                foreach (KeyValuePair<Vector3Int, ExplodingTile> tilePair in explodingTiles)
                    tilePair.Value.ExplodeTile(tilePair.Key, tm);
            }
        }

        source.Play();
    }
    
    public void Bounce(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Dictionary<Vector3Int, BouncyTile> explodingTiles = CollideAt<BouncyTile>(contact.point);
            foreach (KeyValuePair<Vector3Int, BouncyTile> tilePair in explodingTiles)
                if (tilePair.Value != null)
                {
                    tilePair.Value.BounceAt(collision.gameObject, contact.point);
                    return;
                }
        }
    }

    public Dictionary<Vector3Int, T> CollideAt<T>(Vector2 pos)
        where T : TileBase
    {
        Dictionary<Vector3Int, T> tiles = new Dictionary<Vector3Int, T>();
        for (int x = -1; x <= 1; ++x) for (int y = -1; y <= 1; ++y)
            {
                Vector3Int tilePos = tm.WorldToCell(pos + Vector2.up * y * tolerance + Vector2.right * x * tolerance);
                T tile = tm.GetTile<T>(tilePos);
                if (tile != null) tiles[tilePos] = tile;
            }
        return tiles;
    }

    public void ExplodeArea(Vector2 pos, int radius)
    {
        for (int x = 0; x < radius; ++x)
            for (int y = 0; y < radius; ++y)
                if (x * x + y * y < radius)
                {
                    Dictionary<Vector3Int, ExplodingTile> explodingTiles = CollideAt<ExplodingTile>(pos + new Vector2(x, y));
                    foreach (KeyValuePair<Vector3Int, ExplodingTile> tilePair in explodingTiles)
                        tilePair.Value.ExplodeTile(tilePair.Key, tm);
                    explodingTiles = CollideAt<ExplodingTile>(pos + new Vector2(-x, y));
                    foreach (KeyValuePair<Vector3Int, ExplodingTile> tilePair in explodingTiles)
                        tilePair.Value.ExplodeTile(tilePair.Key, tm);
                    explodingTiles = CollideAt<ExplodingTile>(pos + new Vector2(x, -y));
                    foreach (KeyValuePair<Vector3Int, ExplodingTile> tilePair in explodingTiles)
                        tilePair.Value.ExplodeTile(tilePair.Key, tm);
                    explodingTiles = CollideAt<ExplodingTile>(pos + new Vector2(-x, -y));
                    foreach (KeyValuePair<Vector3Int, ExplodingTile> tilePair in explodingTiles)
                        tilePair.Value.ExplodeTile(tilePair.Key, tm);
                }
    }
}
