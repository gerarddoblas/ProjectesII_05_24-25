using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushScript : MonoBehaviour
{
    public bool flipX;
    [SerializeField] private float force;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>() != null)
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force * 0.5f + Vector2.right * force * (flipX ? -1f : 1f));
    }
}
