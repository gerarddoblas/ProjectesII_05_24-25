using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Beam:Item
{
    public float timeInScene = .5f;
    public float contador = 0.0f;
    private AudioSource source;
    [SerializeField] private GameObject particle;

    private void Start()
    {
        AudioManager.instance.PlaySFX("Laser");
        if (creator.transform.position.x > this.transform.position.x)
            this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
    }
    public void Update()
    {

        contador += Time.deltaTime;
        if (contador >= timeInScene)
        {
            Instantiate(particle, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    override public IEnumerator Effect(GameObject target)
    {
        yield return target;
    }
}
