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

    [SerializeField] private GameObject particle;
    [SerializeField] private float particleRate = 5.0f;
    void Start()
    {
        source = GetComponent<AudioSource>();
        ChangeArea();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBeforeChange < 0) ChangeArea();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.Score += (Time.deltaTime * scoreMultiplier);
            player.onAlterScore.Invoke(player.Score);

            if(Random.Range(0f, 100f) < particleRate)
            {
                Vector3 spawnPos = 
                    this.transform.position 
                    + this.transform.localScale.y * 1.5f * Vector3.up 
                    + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

                GameObject instance = Instantiate(particle, spawnPos, Quaternion.identity);
                instance.GetComponent<AreaParticleScript>().objective = collision.gameObject.transform;
            }
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

    public Vector3 GetRandomPositionFromList() => possibleAreas[Random.Range(0, possibleAreas.Length)];

    public void ChangeArea()
    {
        Vector3 curPosition = transform.position;
        do { transform.position = GetRandomPositionFromList(); } while (transform.position == curPosition);
        timeBeforeChange = Random.Range(1.5f, 5.0f);
        source.Play();
    }
}
