using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawning : MonoBehaviour
{
    public static PlayerSpawning Instance { get; private set; }
    public Transform[] positions = new Transform[4];
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        for(int i = 0; i < PlayersManager.Instance.players.Count; i++)
        {
            PlayersManager.Instance.players[i].transform.position = positions[i].position;
            Debug.Log(PlayerSpawning.Instance.positions[i].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
