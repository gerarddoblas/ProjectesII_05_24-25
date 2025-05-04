using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Image timerRect;
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
    }

    public void UpdateTimerRect(float remainingSeconds, float gameTime)
    {
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
