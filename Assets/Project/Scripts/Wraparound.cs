using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraparound : MonoBehaviour
{
    [SerializeField] private BoxCollider2D area;
    [SerializeField] private Transform objective;
    // Start is called before the first frame update
    void Start()
    {
        area = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = new Vector2(objective.transform.position.x, collision.transform.position.y);
    }
}
