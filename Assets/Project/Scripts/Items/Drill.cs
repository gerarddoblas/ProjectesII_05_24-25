using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : Item
{
    private AudioSource source;
    public float timeInScene = 1.5f;
    public float contador = 0.0f;
    private Rigidbody2D rb;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        //rb.velocity = new Vector2(0, -10);
        contador += Time.deltaTime;
        if (contador >= timeInScene)
        {
            Destroy(this.gameObject);
            //AudioManager.instance.PlaySFX("Drill");
        }
    }
    override public IEnumerator Effect(GameObject target)
    {
        //Debug.Log("Collied/Triggered");
        //if (target.TryGetComponent<Player>(out Player p))
        //{
        //    Debug.Log("Mine collied with player");
        //    AudioManager.instance.PlaySFX("Mine");
        //    p.healthBehaviour.Damage(2);
        //    GameController.Instance.RemoveScore(p.healthBehaviour.maxhealth, p.gameObject);
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //    Debug.Log("Mine collied with other object");
        //    if (target.gameObject)
        //    {
        //        AudioManager.instance.PlaySFX("MineInMap");
        //        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //    }
        //}
        yield return null;
    }
}
