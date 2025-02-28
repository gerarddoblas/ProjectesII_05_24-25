using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        Debug.Log("Coin Collided");
        if (player != null)
        {
            Debug.Log("Collided with player");
            player.Score++;
            Destroy(this.gameObject);
        }
    }
}
