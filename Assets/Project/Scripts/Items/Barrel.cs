using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Item
{
    public float timeInScene = 1.5f;
    public float contador = 0.0f;
    public int damage = 15;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Update()
    {
        contador += Time.deltaTime;
        if ( contador >= timeInScene )
        {
            Destroy(this.gameObject);
            source.Play();
        }
    }
    override public IEnumerator Effect(GameObject target)
    {

        if (target.TryGetComponent<Player>(out Player p))
        {
            p.rigidbody2D.velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, target.GetComponent<Rigidbody2D>().velocity.y);
            p.healthBehaviour.Damage(damage);
            Destroy(this.gameObject);
        }
        yield return null;
    }
}
