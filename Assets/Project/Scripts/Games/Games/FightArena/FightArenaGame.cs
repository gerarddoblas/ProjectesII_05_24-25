using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "TimeBasedGame", menuName = "Games/TimeBasedGame/FightArena")]
public class FightArenaGame : TimeBasedGame
{
    override public void StartGame()
    {
        base.StartGame();
        PlayersManager.Instance.SetJoining(false);
        remainingTime = gameTime;
        //LeanTween.alphaCanvas(counterText.GetComponentInParent<CanvasGroup>(), 1, 1);
        PlayersManager.Instance.ShowAllHuds(1);
        PlayersManager.Instance.EnablePlayersCreation();
        /*onFinishGame.AddListener(delegate ()
        {
            PlayersManager.Instance.HideAllHuds();
            PlayersManager.Instance.LockPlayersMovement();
            PlayersManager.Instance.DisablePlayersCreation();
            CameraFX.Instance.VerticalClap(() => {
                SceneManager.LoadScene("ResultScene");
            });
        });*/
    }

    override public void UpdateGame()
    {
        base.UpdateGame();

    }
}

