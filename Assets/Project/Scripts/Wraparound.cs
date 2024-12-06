using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraparound : MonoBehaviour
{
    [SerializeField] private BoxCollider2D area;
    [SerializeField] private Transform objective;
    private GameObject[] wrapedObjects;
    void Start()
    {
        area = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        collision.transform.position = new Vector2(objective.transform.position.x, collision.transform.position.y);
    }
}
