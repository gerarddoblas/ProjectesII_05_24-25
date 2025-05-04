using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class Kaboom : Item
{
    [SerializeField] private float timeInScene = 2f;
    [SerializeField] private float counter = 0.0f;
    [SerializeField] private float growth = .1f;
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
        counter += Time.deltaTime;
        this.transform.localScale = new Vector3(
            this.transform.localScale.x + (growth * Time.deltaTime),
            this.transform.localScale.y + (growth * Time.deltaTime),
            this.transform.localScale.z + (growth * Time.deltaTime)
        );

        if (counter >= timeInScene)
        {
            AudioManager.instance.PlaySFX("Kaboom");
            Destroy(this.gameObject); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<HealthBehaviour>(out HealthBehaviour hb)&&(collision.gameObject!=creator))
            hb.FullDamage();
    }

    override public IEnumerator Effect(GameObject target)
    {
        if (target.TryGetComponent<HealthBehaviour>(out HealthBehaviour hb))
        {
            GameController.Instance.RemoveScore(hb.maxhealth, hb.gameObject);
            hb.FullDamage();
        }
        yield return null;
    }
}
