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

        contador += Time.deltaTime;
        if (contador >= timeInScene)// Comprobar que es funcional las particulas
        {
            Instantiate(particle);
            Destroy(this.gameObject);// He puesto esto para que no este constantemente llamandose xd
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.Score++;
        }
    }
}
