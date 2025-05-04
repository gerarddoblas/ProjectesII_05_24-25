using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Localization.Editor;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Image timerRect;

    [Header("Game Texts")]
    [SerializeField] TMP_Text gameText;
    [SerializeField] TMP_Text coinCollect;
    [SerializeField] TMP_Text fightArena;
    [SerializeField] TMP_Text zoneCapture;
    [SerializeField] TMP_Text stealTheCrown;
    [Header("Warning")]
    [SerializeField] bool warning = true;
    private bool hasWarned = false;
    float timeScinceLastWarning = 0;
    [SerializeField] float remainingTimeForAdvice = 10f;
    public static Timer Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
        timerRect.transform.localScale = Vector3.zero;
        gameText.text = "";
    }

    public void UpdateTimerRect(float remainingSeconds, float gameTime)
    {
        if(gameText.text == "" && GameController.Instance.currentGameMode != null)
        {
            if(GameController.Instance.currentGameMode.GetType().Equals(typeof(CoinCollectGame)))
            {
                gameText.text = coinCollect.text;
            }
            else if(GameController.Instance.currentGameMode.GetType().Equals(typeof(FightArenaGame)))
            {
                gameText.text = fightArena.text;
            }
            else if(GameController.Instance.currentGameMode.GetType().Equals(typeof(StealTheCrown)))
            {
                gameText.text = stealTheCrown.text;
            }
            else if(GameController.Instance.currentGameMode.GetType().Equals(typeof(TimeZoneCapture)))
            {
                gameText.text = zoneCapture.text;
            }
        }

        float scale = remainingSeconds / gameTime;
        timerRect.transform.localScale =
            scale * Vector3.right 
            + Vector3.up 
            + Vector3.forward;
        timerRect.color = Color.white * scale + Color.red * (1 - scale);

        if (remainingSeconds <= remainingTimeForAdvice && warning)
        {
            if (!hasWarned)
            {
                LeanTween.value(0, 1, 1).setOnUpdate(delegate (float r)
                {
                    AudioManager.instance.SetMusicSpeed(1 + (r / 4));
                });
                hasWarned = true;
            }
            timeScinceLastWarning += Time.deltaTime;

            if (timeScinceLastWarning >= 1f)
            {
                timeScinceLastWarning = 0f;
            }
        }
    }

}
