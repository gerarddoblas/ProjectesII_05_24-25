using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] private Vector2[] possibleAreas;
    [SerializeField] private float timeBeforeChange;
    [SerializeField] private float scoreMultiplier = 5f;
    private AudioSource source;
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
        if (timeBeforeChange < 0) ChangeArea();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.Score += (Time.deltaTime * scoreMultiplier);
            player.onAlterScore.Invoke(player.Score);
            timeBeforeChange -= Time.deltaTime;
        }
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
