using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] private Vector2[] possibleAreas;
    [SerializeField] private float timeBeforeChange;
    private AudioSource source;
    [SerializeField] private AudioClip InArea;
   [SerializeField] private float scoreMultiplier = 5f;
    int players = 0;
    public static AreaManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    void Start()
    {
        source = GetComponent<AudioSource>();
        ChangeArea();
    }

    // Update is called once per frame
    void Update()
    {
        timeBeforeChange -= Time.deltaTime;
        if (timeBeforeChange < 0) ChangeArea();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.Score += (Time.deltaTime * scoreMultiplier);
            player.onAlterScore.Invoke(player.Score);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        source.clip = InArea;
        source.Play();
        players++;
    }
    private void OnTriggerExit2D(Collider2D collision)
    { 
        if (--players == 0)
            source.Stop();
    }
    public Vector3 GetRandomPositionFromList() { 
        return possibleAreas[Random.Range(0, possibleAreas.Length)];}
    public void ChangeArea()
    {
        transform.position = GetRandomPositionFromList();
       timeBeforeChange = Random.Range(3.0f, 10.0f);
        source.Play();
    }
}
