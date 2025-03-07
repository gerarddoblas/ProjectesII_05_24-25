using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField]List<GameObject> possibleItems = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Items>(out Items i))
        {
            GameObject item = possibleItems[Random.Range(0,possibleItems.Count)];
            i.recievedObject = item;
            i.onItemRecieved.Invoke(item.GetComponent<SpriteRenderer>().sprite);
            //GameController.Instance.AddScore(10,i.gameObject);
            Destroy(this.gameObject);
        }
    }
}
