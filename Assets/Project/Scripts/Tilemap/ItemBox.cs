using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField]List<GameObject> possibleItems = new List<GameObject>();
    [SerializeField] private GameObject particle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Items>(out Items i))
        {
            GameObject item = possibleItems[Random.Range(0,possibleItems.Count)];
            i.recievedObject = item;
            i.onItemRecieved.Invoke(item.GetComponent<SpriteRenderer>().sprite);
            Instantiate(particle, this.transform.position, Quaternion.identity);
            //GameController.Instance.AddScore(10,i.gameObject);
            //Destroy(this.gameObject);
            StartCoroutine(HideTemporally());
        }
    }
    IEnumerator HideTemporally()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.GetComponent<Animator>().enabled = false;
        this.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(5);

        this.GetComponent<BoxCollider2D>().enabled = true;
        this.GetComponent<Animator>().enabled = true;
        this.GetComponent<SpriteRenderer>().enabled = true;
        yield return null;
    }
}
