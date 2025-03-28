using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour
{
    [Header("minPlayersToPass: 0-AllPlayers")]
    [SerializeField] int minPlayersToPass;
    [SerializeField] bool activateClapAnimation;
    private int players;

    [SerializeField] float timeToAccept;
    float timer;
    [SerializeField] Image timerImage;
    [SerializeField] TextMeshProUGUI timerText;

    private Coroutine countdownCoroutine;
    private Coroutine refillCoroutine;

    bool accepted = false;

    private void Start()
    {
        timer = timeToAccept;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        players++;

        if (refillCoroutine != null)
        {
            StopCoroutine(refillCoroutine);
            refillCoroutine = null;
        }

        if (countdownCoroutine == null &&
            ((players == PlayersManager.Instance.players.Count && minPlayersToPass == 0) ||
             (players >= minPlayersToPass && minPlayersToPass != 0)))
        {
            StartCountdown();
        }
    }
    void StartCountdown()
    {
        countdownCoroutine = StartCoroutine(StartCountdown(delegate ()
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
        }));
    }
    IEnumerator StartCountdown(UnityAction onComplete)
    {
        if(timer == timeToAccept)
        {
            timerImage.fillAmount = 1;
            timerText.text = ((int)timer).ToString();
        }
        yield return null;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerImage.fillAmount = timer / timeToAccept;
            timerText.text = ((int)timer + 1).ToString();
            yield return null;
        }
        timerImage.fillAmount = 0;
        timerText.text = "";
        onComplete();
        countdownCoroutine = null;
    }
    private void StopCountdown()
    {

    }
    IEnumerator RefillTimer()
    {
        while (timer <= timeToAccept)
        {
            timer += Time.deltaTime;
            timerImage.fillAmount = timer / timeToAccept;
            timerText.text = ((int)timer + 1).ToString();
            if (timer >= timeToAccept)
            {
                timer = timeToAccept;
                timerImage.fillAmount = 0;
                timerText.text = "";
            }
            yield return null;
        }
        timer = timeToAccept;
        timerImage.fillAmount = 0;
        timerText.text = "";
        refillCoroutine = null;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        players--;

        // Si ya no queda nadie, detener cuenta regresiva y recargar
        if (players <= 0)
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
            }

            if (refillCoroutine == null)
                refillCoroutine = StartCoroutine(RefillTimer());

        }
    }
}
