using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam:Item
{
    public float timeInScene = .5f;
    public float contador = 0.0f;

    private void Start()
    {
        if (creator.transform.position.x > this.transform.position.x)
            this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
    }
    public void Update()
    {
        contador += Time.deltaTime;
        if (contador >= timeInScene)
        {
            Destroy(this.gameObject);
        }
    }
    override public void Effect(GameObject target)
    {

    }
}
