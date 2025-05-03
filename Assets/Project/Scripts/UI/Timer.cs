using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public enum ETimeFormat
    {
        HOURMINSEC,
        MINUTEMINSEC,
        SECONDS
    }
    public enum EShowOptions
    {
        ONLYGREATERTHANZERO,
        POSITIVE,
        NEGATIVE
    }
    [SerializeField]TextMeshProUGUI timerText;
    [Header("Timer display options")]
    public bool showMS;
    public ETimeFormat timeFormat; 
    public EShowOptions showOptions = EShowOptions.POSITIVE;
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
        if (timerText == null)
            timerText = this.GetComponent<TextMeshProUGUI>();
    }

    public void StartTimer(float seconds)
    {
        LeanTween.scale(transform.GetChild(0).gameObject, new Vector3(0, 1, 1), seconds);
    }
    public void UpdateTimerText(float remainingSeconds)
    {
        string timeString = "";

        int currentSecond = Mathf.FloorToInt(remainingSeconds);


        if ((remainingSeconds >= 0 && showOptions == EShowOptions.POSITIVE) ||
            (remainingSeconds > 0 && showOptions == EShowOptions.ONLYGREATERTHANZERO) ||
            (showOptions == EShowOptions.NEGATIVE))
        {
            switch (timeFormat)
            {
                case ETimeFormat.MINUTEMINSEC:
                    timeString = (remainingSeconds / 60).ToString().Split(",")[0] + (remainingSeconds % 60).ToString();
                    break;
                default:
                    timeString = remainingSeconds.ToString().Split(",")[0];
                    break;
            }
            if (showMS)
                timeString += "." + remainingSeconds.ToString().Split(",")[1];

            if (remainingSeconds <= remainingTimeForAdvice && warning)
            {
                if (!hasWarned)
                {
                    LeanTween.value(0, 1, 1).setOnUpdate(delegate (float r)
                    {
                        timerText.color = new Color(1 - (r / 10), 1 - r, 1 - r);
                        AudioManager.instance.SetMusicSpeed(1 + (r / 4));
                    });
                    hasWarned = true;
                }
                timeScinceLastWarning += Time.deltaTime; 

                if (timeScinceLastWarning >= 1f)
                {
                    timeScinceLastWarning = 0f; 

                    timerText.transform.localScale = Vector3.one; 
                    LeanTween.scale(timerText.gameObject, Vector3.one * 1.5f, 0.5f)
                        .setEaseOutBounce()
                        .setOnComplete(() => {
                            LeanTween.scale(timerText.gameObject, Vector3.one, 0.2f).setEaseInCubic();
                        });
                }
            }
        }
        timerText.text = timeString;
    }

}
