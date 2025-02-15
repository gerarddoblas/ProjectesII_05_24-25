using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaParticleScript : MonoBehaviour
{
    public Transform objective;
    public float speed = 1f;
    public bool scaleSpeed = false;
    [SerializeField] private float tolerance = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = objective.position - transform.position;

        if (direction.magnitude < tolerance) Destroy(this.gameObject);
        else transform.position += direction.normalized * speed * ((scaleSpeed) ? direction.magnitude : 1.0f);
    }
}
