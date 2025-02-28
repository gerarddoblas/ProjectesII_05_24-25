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

    [SerializeField]TextMeshProUGUI timerText;
    public bool showMS;
    public ETimeFormat timeFormat; 
    
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

    public void UpdateTimerText(float remainingSeconds)
    {
        string timeString = "";
        switch (timeFormat)
        {
            case ETimeFormat.MINUTEMINSEC:
                timeString = (remainingSeconds/60).ToString().Split(",")[0] + (remainingSeconds%60).ToString();
                break;
            default: 
                timeString = remainingSeconds.ToString().Split(",")[0];
                break;
        }
        if (showMS)
            timeString += "." + remainingSeconds.ToString().Split(",")[1];
        timerText.text = timeString;
    }
}
