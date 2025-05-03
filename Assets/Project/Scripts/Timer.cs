using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Image timerRect;
    //public enum ETimeFormat
    //{
    //    HOURMINSEC,
    //    MINUTEMINSEC,
    //    SECONDS
    //}
    //public enum EShowOptions
    //{
    //    ONLYGREATERTHANZERO,
    //    POSITIVE,
    //    NEGATIVE
    //}
    //[SerializeField]TextMeshProUGUI timerText;
    //[Header("Timer display options")]
    //public bool showMS;
    //public ETimeFormat timeFormat; 
    //public EShowOptions showOptions = EShowOptions.POSITIVE;
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
        //if (timerText == null)
        //    timerText = this.GetComponent<TextMeshProUGUI>();
    }

    public void StartTimer(float seconds)
    {
        LeanTween.scale(transform.GetChild(0).gameObject, new Vector3(0, 1, 1), seconds);
    }

    public void UpdateTimerRect(float remainingSeconds, float gameTime)
    {
        float scale = remainingSeconds / gameTime;
        timerRect.transform.localScale =
            scale * Vector3.right 
            + timerRect.transform.localScale.y * Vector3.up 
            + timerRect.transform.localScale.z * Vector3.forward;
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
