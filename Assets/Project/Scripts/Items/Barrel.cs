using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Item
{
    public float timeInScene = 1.5f;
    public float contador = 0.0f;
    public int damage = 15;
    private AudioSource source;
    [SerializeField] private AudioClip BarrelBoom;
    private Rigidbody2D rb;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        contador += Time.deltaTime;
        if ( contador >= timeInScene )
        {
            AudioManager.instance.PlaySFX("Barrel");
            Destroy(this.gameObject);

        }

        rb.velocity = Vector3.up * rb.velocity.y + Vector3.right * Mathf.Sign(rb.velocity.x) * 15;
        transform.eulerAngles += Vector3.forward * Time.deltaTime;
    }
    override public IEnumerator Effect(GameObject target)
    {

        if (target.TryGetComponent<Player>(out Player p))
        {
            //p.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, target.GetComponent<Rigidbody2D>().velocity.y);

            AudioManager.instance.PlaySFX("Barrel");
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, target.GetComponent<Rigidbody2D>().velocity.y);
            p.healthBehaviour.Damage(damage);
            GameController.Instance.RemoveScore(damage,p.gameObject);
            Destroy(this.gameObject);

        }
        yield return null;
    }
}
