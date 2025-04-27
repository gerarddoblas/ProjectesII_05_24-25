using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TimeBasedGame", menuName = "Games/TimeBasedGame/CoinCollect")]
public class CoinCollectGame : TimeBasedGame
{
    override public void StartGame()
    {
        base.StartGame();
        PlayersManager.Instance.SetJoining(false);
        remainingTime = gameTime;
        PlayersManager.Instance.ShowAllHuds(1);
        PlayersManager.Instance.EnablePlayersCreation();
    }

    override public void UpdateGame()
    {
        base.UpdateGame();

    }
}

