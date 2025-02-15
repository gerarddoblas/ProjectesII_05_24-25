using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StageSelector : MonoBehaviour
{
    [SerializeField] string[] stages ;
    [Header("minPlayersToPass: 0-AllPlayers")]
    [SerializeField] int minPlayersToPass;
    [SerializeField]bool  activateClapAnimation;
    private int players;
    public void LoadRandomStage()
    {        
        SceneManager.LoadScene(stages[(int)Random.Range(0,stages.Length-1)]);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        players++;
        if ((players == PlayersManager.Instance.players.Count&&minPlayersToPass==0)||
            (players>=minPlayersToPass&&minPlayersToPass!=0))
        {
            if (activateClapAnimation)
            {
                CameraFX.Instance.VerticalClap(delegate ()
                {
                    Debug.Log("LoadingRandomStage...");
                    LoadRandomStage();
                });
            }
            else
            {
                Debug.Log("LoadingRandomStage...");
                LoadRandomStage();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        players--;
    }
}
