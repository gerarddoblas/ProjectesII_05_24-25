using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraparound : MonoBehaviour
{
    [SerializeField] private BoxCollider2D area;
    [SerializeField] private Wraparound objectiveWraparound;
    [SerializeField] private GameObject particles;
    public List<Collider2D> objectsInside = new List<Collider2D>();

    void Start()
    {
        area = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() == null) return;
        if (!objectsInside.Contains(collision) && !objectiveWraparound.objectsInside.Contains(collision))
        {
            objectsInside.Add(collision);
            Instantiate(particles, collision.transform.position, Quaternion.identity);
            collision.transform.position = new Vector2(objectiveWraparound.transform.position.x, collision.transform.position.y);
            Instantiate(particles, collision.transform.position, Quaternion.identity);
            objectiveWraparound.objectsInside.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (objectsInside.Contains(collision))
            objectsInside.Remove(collision);
    }
}
