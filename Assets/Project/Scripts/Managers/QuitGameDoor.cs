using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class QuitGameDoor : MonoBehaviour
{
    [SerializeField] bool activateClapAnimation;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        GameObject player = collision.gameObject;
        if (PlayersManager.Instance.players.Contains(player))
        {
            player.GetComponent<Player>().LockMovement();
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;

            LeanTween.scale(player.gameObject, Vector3.zero, .5f).setOnStart(() => {
                LeanTween.move(player.gameObject, this.transform.position, .1f);
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
                    if (activateClapAnimation) CameraFX.Instance.VerticalClap(delegate () { Quit(); });
                    else Quit();
                }
            });
        }
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
