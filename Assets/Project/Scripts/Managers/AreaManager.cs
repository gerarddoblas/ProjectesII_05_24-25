using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] private int curAreaIndex;
    [SerializeField] private Vector2[] possibleAreas;
    [SerializeField] private float timeBeforeChange;
    private AudioSource source;
    private ParticleSystem particleSystem;
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
        particleSystem = GetComponentInChildren<ParticleSystem>();
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
            //player.Score += ();
            //player.onAlterScore.Invoke(player.Score);
            GameController.Instance.AddScore(Time.deltaTime * scoreMultiplier, collision.gameObject);
            timeBeforeChange -= Time.deltaTime;

            if(Random.Range(0f, 100f) < particleRate)
            {
                Vector3 spawnPos = 
                    this.transform.position 
                    + this.transform.localScale.y * 1.5f * Vector3.up 
                    + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

                GameObject instance = Instantiate(particle, spawnPos, Quaternion.identity);
                instance.GetComponent<AreaParticleScript>().objective = collision.gameObject.transform;
                instance.GetComponent<AreaParticleScript>().speed = 1.0f;
                instance.GetComponent<AreaParticleScript>().scaleSpeed = true;
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
        players--;
        if (players == 0)
            source.Stop();
    }

    public int GetRandomIndex() => Random.Range(0, possibleAreas.Length);
    public Vector3 GetRandomAreaPosition() => possibleAreas[GetRandomIndex()];

    public void ChangeArea()
    {
        timeBeforeChange = Random.Range(1.5f, 5.0f);

        Vector3 curPosition = transform.position;
        int curIndex = curAreaIndex;
        do { curIndex = GetRandomIndex(); } while (curIndex == curAreaIndex);

        transform.position = possibleAreas[curIndex];
        curAreaIndex = curIndex;

        int particleCount = Random.Range(5, 8);
        while(particleCount > 0)
        {
            GameObject instance = Instantiate(
                particle, 
                curPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f), 
                Quaternion.identity
                );
            instance.GetComponent<AreaParticleScript>().objective = this.transform;
            instance.GetComponent<AreaParticleScript>().speed = .5f;
            instance.GetComponent<AreaParticleScript>().scaleSpeed = false;
            --particleCount;
        }

        //particleSystem.Play();
        source.Play();
    }
}
