using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [Header("minPlayersToPass: 0-AllPlayers")]
    [SerializeField] int minPlayersToPass;
    [SerializeField] bool activateClapAnimation;
    private int players;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        players++;
        if ((players == PlayersManager.Instance.players.Count && minPlayersToPass == 0) ||
            (players >= minPlayersToPass && minPlayersToPass != 0))
        {
            if (activateClapAnimation)
            {
                CameraFX.Instance.VerticalClap(delegate ()
                {
                    Debug.Log("StartingGames...");
                    GameController.Instance.StartGames();
                });
            }
            else
            {
                Debug.Log("StartingGames...");
               GameController.Instance.StartGames();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        players--;
    }
}
