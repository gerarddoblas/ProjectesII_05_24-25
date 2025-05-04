using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBasedGame : BaseGame
{
    [SerializeField] protected float gameTime;
    [SerializeField]protected float remainingTime;
    override public void  StartGame(){ 
        base.StartGame();
        remainingTime = gameTime;
    }
    
    override public void UpdateGame()
    {
        if (!playingGame) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
            FinishGame();
        try
        {
            Timer.Instance.UpdateTimerRect(remainingTime, gameTime);
        }
        catch (Exception e) { }
    }
    public override void FinishGame(){
        base.FinishGame();
    }
}
