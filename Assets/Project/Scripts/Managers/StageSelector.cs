using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
        if(stages.Length == 0)
        {

#if UNITY_EDITOR
            EditorApplication.isPlaying = false; return;
#else
            Application.Quit(); return;
#endif
        }
        SceneManager.LoadScene(stages[(int)Random.Range(0,stages.Length-1)]);
    }
    /* private void OnTriggerEnter2D(Collider2D collision)
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
     }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject player = collision.gameObject;
        if (PlayersManager.Instance.players.Contains(player))
        {
            player.GetComponent<Player>().LockMovement();
            LeanTween.scale(player.gameObject, Vector3.zero, .5f).setOnStart(() =>
            {
                //LeanTween.move(player.gameObject, this.transform.position, .5f);
            }).setOnComplete(() =>
            {
                int index = PlayersManager.Instance.players.IndexOf(player);
                GameObject hud = PlayersManager.Instance.playersCanvas[index];

                Destroy(PlayersManager.Instance.playersCanvas[index]);
                PlayersManager.Instance.playersCanvas[index] = null;

                Destroy(player);
                PlayersManager.Instance.players[index] = null;


                if (PlayersManager.Instance.players.Count == 0 || PlayersManager.Instance.players.Count(p => p != null) == 0)
                {
                    PlayersManager.Instance.SetJoining(false);
                    if (activateClapAnimation)
                    {
                        CameraFX.Instance.VerticalClap(delegate ()
                        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                            Application.Quit();
#endif
                        });
                    }
                    else
                    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    }
                }
            });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        players--;
    }
}
