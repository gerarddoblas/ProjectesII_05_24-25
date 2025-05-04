using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameDoor : MonoBehaviour
{
    [Header("minPlayersToPass: 0-AllPlayers")]
    [SerializeField] private int minPlayersToPass;
    [SerializeField] private bool activateClapAnimation;
    private int players;

    [SerializeField] private float timeToAccept;
    private float timer;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        timer = timeToAccept;
    }

    private void Update()
    {
        if (players == 0 || !((players == PlayersManager.Instance.players.Count && minPlayersToPass == 0) ||
             (players >= minPlayersToPass && minPlayersToPass != 0))) return;

        if(timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = ((int)timer + 1).ToString();
            timerText.transform.localScale = Vector3.one * (timer - Mathf.Floor(timer)); //Scale by fractional part
            timerText.color = new Color(timerText.color.r, timerText.color.g, timerText.color.b,(timer - Mathf.Floor(timer)));
            return;
        }

        timerText.text = "";
        timerText.transform.localScale = Vector3.one;
        timerText.color = new Color(timerText.color.r, timerText.color.g, timerText.color.b, (timer - Mathf.Floor(timer)));
        CameraFX.Instance.VerticalClap();
        GameController.Instance.StartGames();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponent<Player>()) return;

        GameObject player = collision.gameObject;
        int index = PlayersManager.Instance.players.IndexOf(player);
        GameObject hud = PlayersManager.Instance.playersCanvas[index];

        LeanTween.scale(hud.GetComponent<PlayerHud>().readyText.gameObject, Vector3.one * 1.5f, .25f).setOnComplete( delegate() { 
            LeanTween.scale(hud.GetComponent<PlayerHud>().readyText.gameObject, Vector3.one, .25f); 
        });

        players++;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.GetComponent<Player>()) return;

        GameObject player = collision.gameObject;
        int index = PlayersManager.Instance.players.IndexOf(player);
        GameObject hud = PlayersManager.Instance.playersCanvas[index];

        LeanTween.scale(hud.GetComponent<PlayerHud>().readyText.gameObject, Vector3.zero, .25f);

        players--;
        //If no one is left, reset
        if (players <= 0)
        {
            timer = timeToAccept;
            timerText.text = "";
        }
    }
}
