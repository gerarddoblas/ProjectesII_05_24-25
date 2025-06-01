using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "TimeBasedGame", menuName = "Games/TimeBasedGame/ZoneCapture")]
public class TimeZoneCapture : TimeBasedGame {
    override public void StartGame()
    {
        base.StartGame();
        PlayersManager.Instance.SetJoining(false);
        remainingTime = gameTime;
        PlayersManager.Instance.ShowAllHuds(1);
        PlayersManager.Instance.EnablePlayersCreation();
        Timer.Instance.gameText.text = Timer.Instance.zoneCapture.text;
    }
    
    override public void UpdateGame()
    {
        base.UpdateGame();
        
    }
}
