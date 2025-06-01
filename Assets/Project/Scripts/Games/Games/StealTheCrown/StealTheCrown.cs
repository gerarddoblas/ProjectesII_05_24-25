using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StealTheCrown", menuName = "Games/TimeBasedGame/StealTheCrown")]
public class StealTheCrown : TimeBasedGame
{
    [SerializeField] GameObject crownPrefab;
    public GameObject instantiatedCrown;
    private GameObject CrownOwner;
    [SerializeField] private bool scoreOverTime = false;
    [SerializeField] private int scoreToAdd = 50;
    override public void StartGame()
    {
        base.StartGame();
        PlayersManager.Instance.SetJoining(false);
        remainingTime = gameTime;
        PlayersManager.Instance.ShowAllHuds(1);
        PlayersManager.Instance.EnablePlayersCreation();
        Timer.Instance.gameText.text =Timer.Instance.stealTheCrown.text;
    }

    override public void UpdateGame()
    {
        base.UpdateGame();
        GameController.Instance.AddScore(scoreToAdd * Time.deltaTime, Crown.Instance.GetOwner().gameObject);
    }
    
}
