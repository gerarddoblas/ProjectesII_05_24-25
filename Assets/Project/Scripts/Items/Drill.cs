using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : Item
{
    private AudioSource source;
    [SerializeField] private float timeInScene = 1.5f;
    [SerializeField] private float counter = 0.0f;
    private Rigidbody2D rb;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        AudioManager.instance.PlaySFX("Drill");
    }
    public void Update()
    {
        counter += Time.deltaTime;
        if (counter >= timeInScene)
            Destroy(this.gameObject);
    }
    override public IEnumerator Effect(GameObject target)
    {
        yield return null;
    }
}
