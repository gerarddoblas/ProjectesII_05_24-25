using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerWallScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Tilemap>() == null) return;
        Debug.Log("Colliding with playerwall");
        rb.position += Vector2.down * speed * Time.deltaTime;
    }
}
