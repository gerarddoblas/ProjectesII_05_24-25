using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class BaseGame : ScriptableObject
{
    
    protected bool playingGame = false;
    public virtual void StartGame() { playingGame = true; }
    public virtual void UpdateGame() {}
    public virtual void FinishGame()
    {
        playingGame = false;
        GameController.Instance.NextGame();
    } 
}
