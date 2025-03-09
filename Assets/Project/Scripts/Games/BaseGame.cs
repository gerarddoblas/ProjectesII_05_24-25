using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseGame : ScriptableObject
{
    public Sprite instructions;
    protected bool playingGame = false;
    public void SetGameState(bool newState) { playingGame = newState; }
    public virtual void StartGame() {
        playingGame = true;
        PlayersManager.Instance.SetJoining(false);
        PlayersManager.Instance.ShowAllHuds();
        PlayersManager.Instance.EnablePlayersCreation();
    }
    public virtual void UpdateGame() {}
    public virtual void FinishGame()
    {
        playingGame = false;
        GameController.Instance.NextGame();
    } 
}
