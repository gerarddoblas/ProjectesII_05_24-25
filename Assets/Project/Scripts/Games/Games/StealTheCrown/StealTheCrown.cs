using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TimeBasedGame", menuName = "Games/TimeBasedGame/CoinCollect")]
public class StealTheCrown : TimeBasedGame
{
    [SerializeField] GameObject crownPrefab;
    public GameObject instantiatedCrown;
    private GameObject CrownOwner;
    override public void StartGame()
    {
        base.StartGame();
        
    }

    override public void UpdateGame()
    {
        base.UpdateGame();


    }
}
