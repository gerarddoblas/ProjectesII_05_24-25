using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mina : Item
{
    private AudioSource source;
    [SerializeField]private AudioClip SetMine;
    [SerializeField]private AudioClip MineBoom;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    override public IEnumerator Effect(GameObject target)
    {
        if (target.TryGetComponent<Player>(out Player p))
        {
            AudioManager.instance.PlaySFX("Mine");
            p.healthBehaviour.Damage(2);
            Destroy(this.gameObject);
        }
        else
        {
            if (target.gameObject)
            {
                AudioManager.instance.PlaySFX("MineInMap");
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        yield return null;
    }
}
