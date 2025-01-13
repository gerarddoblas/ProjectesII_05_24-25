using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mina : Item
{
    private AudioSource source;
    [SerializeField]private AudioClip SetMine;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    override public IEnumerator Effect(GameObject target)
    {
        if (target.TryGetComponent<Player>(out Player p))
        {
            source.Play(); ;
            p.healthBehaviour.Damage(2);
            Destroy(this.gameObject);
        }
        else
        {
            if (target.gameObject)
            {
                source.clip = SetMine;
                source.Play(); ;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        yield return null;
    }
}
