using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class Kaboom : Item
{
    public float timeInScene = 2f;
    public float contador = 0.0f;
    public float growth = .1f;
    private AudioSource source;
    [SerializeField] private GameObject explosionParticles;
    private void Start()
    {
        TilemapScript.Instance.ExplodeArea(this.transform.position, 100);
        Instantiate(explosionParticles, this.transform.position, Quaternion.identity);
        source = GetComponent<AudioSource>();
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void Update()
    {
        contador += Time.deltaTime;
        this.transform.localScale = new Vector3(
            this.transform.localScale.x + (growth * Time.deltaTime),
            this.transform.localScale.y + (growth * Time.deltaTime),
            this.transform.localScale.z + (growth * Time.deltaTime)
        );

        if (contador >= timeInScene)
        {
            
            AudioManager.instance.PlaySFX("Kaboom");
            Destroy(this.gameObject); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colliding with " + collision.gameObject.name);
        TilemapScript.Instance.ExplodeArea(this.transform.position, (int)Math.Sqrt(this.transform.localScale.magnitude)/2);
        if (collision.TryGetComponent<HealthBehaviour>(out HealthBehaviour hb)&&(collision.gameObject!=creator))
            hb.FullDamage();
    }

    override public IEnumerator Effect(GameObject target)
    {
        TilemapScript.Instance.ExplodeArea(this.transform.position, (int)Math.Sqrt(this.transform.localScale.magnitude)/2);
        Debug.Log("Colliding with " + target.gameObject.name);
        if (target.TryGetComponent<HealthBehaviour>(out HealthBehaviour hb))
        {
            GameController.Instance.RemoveScore(hb.maxhealth, hb.gameObject);
            hb.FullDamage();
        }
        yield return null;
    }
}
