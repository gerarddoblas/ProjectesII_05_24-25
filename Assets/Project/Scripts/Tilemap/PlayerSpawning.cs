using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawning : MonoBehaviour
{
    public static PlayerSpawning Instance { get; private set; }
    public Transform[] positions = new Transform[4];

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        for(int i = 0; i < PlayersManager.Instance.players.Count; i++)
            PlayersManager.Instance.players[i].transform.position = positions[i].position;
    }
}
