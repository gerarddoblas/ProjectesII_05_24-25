using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "TimeBasedGame", menuName = "Games/TimeBasedGame/FightArena")]
public class FightArenaGame : TimeBasedGame
{
    override public void StartGame()
    {
        base.StartGame();
        PlayersManager.Instance.SetJoining(false);
        remainingTime = gameTime;
        PlayersManager.Instance.ShowAllHuds(1);
        PlayersManager.Instance.EnablePlayersCreation();
        Timer.Instance.gameText.text = Timer.Instance.fightArena.text;
    }

    override public void UpdateGame()
    {
        base.UpdateGame();

    }
}

