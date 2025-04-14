using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GravityGrenade : Item {
    public float timeInScene = 1.5f;
    public float contador = 0.0f;
    public int damage = 5;
    [SerializeField] private float force = 500f;
    [SerializeField] private GameObject particles;
    private AudioSource source;

    private void Start () 
    {
        source = GetComponent<AudioSource>();
        this.GetComponent<CircleCollider2D>().enabled = false;
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
        this.GetComponent<CircleCollider2D>().enabled = true;
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3 (90, 0, 0);
        Instantiate(particles, this.transform.position, rotation);
        yield return null;
     }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player p))
        {
            p.gameObject.GetComponent<Rigidbody2D>().AddForce((this.transform.position - p.transform.position).normalized * force);
        }
    }
}
