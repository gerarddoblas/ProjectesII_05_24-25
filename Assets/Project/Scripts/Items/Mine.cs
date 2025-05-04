using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Mine : Item
{
    private AudioSource source;
    [SerializeField]private AudioClip SetMine;
    [SerializeField]private AudioClip MineBoom;
    [SerializeField] private GameObject particles;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    override public IEnumerator Effect(GameObject target)
    {
        if (target.TryGetComponent<Player>(out Player p))
        {
            Instantiate(particles, this.transform.position, Quaternion.identity);
            AudioManager.instance.PlaySFX("Mine");
            p.healthBehaviour.Damage(2);
            GameController.Instance.RemoveScore(p.healthBehaviour.maxhealth, p.gameObject);
            Destroy(this.gameObject);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (target.gameObject)
            {
                AudioManager.instance.PlaySFX("MineInMap");
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        yield return null;
    }
}
