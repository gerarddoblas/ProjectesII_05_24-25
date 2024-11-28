using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] private Vector2[] possibleAreas;
    [SerializeField] private float timeBeforeChange;
    private AudioSource source;
    [SerializeField]private float scoreMultiplier = 5f;
    void Start()
    {
        source = GetComponent<AudioSource>();  
        ChangeArea();
    }

    // Update is called once per frame
    void Update()
    {
        timeBeforeChange -= Time.deltaTime;
        if(timeBeforeChange < 0) ChangeArea();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.Score += (Time.deltaTime*scoreMultiplier);
            player.onAlterScore.Invoke(player.Score);
        }
    }

    public void ChangeArea()
    {
        transform.position = possibleAreas[Random.Range(0, possibleAreas.Length)];
        timeBeforeChange = Random.Range(3.0f, 10.0f);
        source.Play();
    }
}
