using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Egg : Item {
    public float timeInScene = 1.5f;
    public float contador = 0.0f;
    public int damage = 5;
    private AudioSource source;

    private void Start () 
    {
        source = GetComponent<AudioSource>();
    }
    public void Update()
    {
        contador += Time.deltaTime;
        if (contador >= timeInScene)
        {
            Destroy(this.gameObject);
            AudioManager.instance.PlaySFX("Egg");
        }
    }
    override public IEnumerator Effect(GameObject target)
    {
        if (target.TryGetComponent<Tilemap>(out Tilemap tm))
            tm.GetTile(new Vector3Int((int)transform.position.x,(int)transform.position.y));

        if (target.TryGetComponent<Player>(out Player p))
        {
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, target.GetComponent<Rigidbody2D>().velocity.y);
            p.healthBehaviour.Damage(damage);
            GameController.Instance.RemoveScore(damage, p.gameObject);
            Destroy(this.gameObject);
            AudioManager.instance.PlaySFX("Egg");
        }
        yield return null;
     }
}
