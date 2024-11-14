using System.Collections;
using System.Collections.Generic;
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
        tm.RefreshTile(new Vector3Int((int)Mathf.Ceil(collision.contacts[0].point.x) - 1, (int)(collision.contacts[0].point.y - 1f), 0));
        source.Play();
        //TODO: Check edge cases
    }
}
