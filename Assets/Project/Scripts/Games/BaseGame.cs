using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
public class BaseGame : ScriptableObject
{
    
    public LocalizedAsset<Sprite> instructions;
    protected bool playingGame = false;

    public bool gamePaused = false;
    public void SetGameState(bool newState) { playingGame = newState; }
    public virtual void StartGame() {
        playingGame = true;
        PlayersManager.Instance.SetJoining(false);
        PlayersManager.Instance.ShowAllHuds(); 
        PlayersManager.Instance.RemovePlayersItems();
        PlayersManager.Instance.EnablePlayersCreation();
    }
    public virtual void UpdateGame() {}
    public virtual void FinishGame()
    {

        playingGame = false;
        GameController.Instance.NextGame();
        PlayersManager.Instance.RemovePlayersItems();
        PlayersManager.Instance.DisablePlayersCreation();
    } 
}
