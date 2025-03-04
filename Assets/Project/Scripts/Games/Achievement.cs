using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{

    public struct Result
    {
        public Player player;
        public string name;
        public int score;
    }

    private List<Player> players;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Check(Player player)
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Player player in players)
        {

        }
    }
}
