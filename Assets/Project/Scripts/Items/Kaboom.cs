using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        source.Play();
        if (contador >= timeInScene)
        {
            Destroy(this.gameObject);
        }
    }
    override public IEnumerator Effect(GameObject target)
    {
        if(target.TryGetComponent<Tilemap>(out Tilemap tm))
                tm.RefreshTile(new Vector3Int((int)Mathf.Floor(target.transform.position.x ), (int)Mathf.Floor(target.transform.position.y), 0));
        else if (target.TryGetComponent<HealthBehaviour>(out HealthBehaviour hb))
            hb.FullDamage();
        yield return null;
    }
}
