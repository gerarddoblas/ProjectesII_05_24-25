using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public float timeInScene = .5f;
    public float contador = 0.0f;
    private AudioSource source;
    [SerializeField] private GameObject particle;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Player>(out Player player)) return;

        AudioManager.instance.PlaySFX("CollectCoin");
        Instantiate(particle, this.transform.position, Quaternion.identity);
        GameController.Instance.AddScore(1, player.gameObject);
        //Destroy(this.gameObject);
        StartCoroutine(Reapear());
    }
    IEnumerator Reapear()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<CircleCollider2D>().enabled = false; 
        yield return new WaitForSeconds(20);
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<CircleCollider2D>().enabled = true;
        yield return null;
    }
}
