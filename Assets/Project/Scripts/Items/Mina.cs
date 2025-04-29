using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Mina : Item
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
        Debug.Log("Collied/Triggered");
        if (target.TryGetComponent<Player>(out Player p))
        {
            Instantiate(particles, this.transform.position, Quaternion.identity);
            Debug.Log("Mine collied with player");
            AudioManager.instance.PlaySFX("Mine");
            p.healthBehaviour.Damage(2);
            GameController.Instance.RemoveScore(p.healthBehaviour.maxhealth, p.gameObject);
            Destroy(this.gameObject);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Debug.Log("Mine collied with other object");
            if (target.gameObject)
            {
                AudioManager.instance.PlaySFX("MineInMap");
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        yield return null;
    }
}
