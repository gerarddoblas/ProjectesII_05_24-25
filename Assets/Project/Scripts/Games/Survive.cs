using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeBasedGame", menuName = "Games/TimeBasedGame/Survival")]
public class Survive : TimeBasedGame
{
    override public void StartGame()
    {
        base.StartGame();
        PlayersManager.Instance.SetJoining(false);
        remainingTime = gameTime;
    }

    override public void UpdateGame()
    {
        base.UpdateGame();
    }
    public override void FinishGame()
    {
        for (int i = 0; i < PlayersManager.Instance.players.Count; i++) {
            Player p = PlayersManager.Instance.players[i].GetComponent<Player>();
            if (!p.GetKO())
                GameController.Instance.AddScore(50,p.gameObject);
        }
        base.FinishGame();
    }
}
