using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public float timeInScene = .5f;
    public float contador = 0.0f;
    private AudioSource source;
    [SerializeField] private GameObject particle;
    // Start is called before the first frame update
    private void Start()
    {
       // AudioManager.instance.PlaySFX("Laser");

    }

    // Update is called once per frame
    void Update()
    {
        if(!GameController.Instance.currentGameMode.GetType().Equals(typeof(CoinCollectGame)))
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.instance.PlaySFX("CollectCoin");
        Player player = collision.GetComponent<Player>();
        Debug.Log("Coin Collided");
        if (player != null)
        {
            Debug.Log("Collided with player");
            player.Score++;
            Instantiate(particle, this.transform.position, Quaternion.identity);
            GameController.Instance.AddScore(1, player.gameObject);
            Destroy(this.gameObject);
        }
    }
}
