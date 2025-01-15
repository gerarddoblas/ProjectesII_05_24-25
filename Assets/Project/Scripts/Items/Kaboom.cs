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
    public float growth = .25f;
    private AudioSource source;
    private void Start()
    {
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
        try
        {
            GameObject.Find("Grid").GetComponentInChildren<TilemapScript>().ExplodeArea(this.transform.position, (int)(transform.localScale.x * transform.localScale.y));
        } catch (Exception e) { }
        if (contador >= timeInScene)
        {
            
            //AudioManager.instance.PlaySFX("Kaboom");
            Destroy(this.gameObject); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colliding with " + collision.gameObject.name);
        TilemapScript.Instance.ExplodeArea(this.transform.position, (int)Math.Sqrt(this.transform.localScale.magnitude));
    }

    override public IEnumerator Effect(GameObject target)
    {
        TilemapScript.Instance.ExplodeArea(this.transform.position, (int)Math.Sqrt(this.transform.localScale.magnitude));
        
        if (target.TryGetComponent<HealthBehaviour>(out HealthBehaviour hb))
            hb.FullDamage();
        yield return null;
    }
}
