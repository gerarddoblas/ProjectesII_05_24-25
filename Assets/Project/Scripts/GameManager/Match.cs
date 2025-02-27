using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Match : MonoBehaviour
{
    public float gameTime = 100;
    private float remainingTime;
    public TextMeshProUGUI counterText;
    public UnityEvent onFinishGame;
    private AudioSource source;
    [SerializeField] private AudioClip ShortGameTime;
    
    void OnEnable()
    {
        PlayersManager.Instance.SetJoining(false);
        remainingTime = gameTime;
        LeanTween.alphaCanvas(counterText.GetComponentInParent<CanvasGroup>(),1,1);
        PlayersManager.Instance.ShowAllHuds(1);
        PlayersManager.Instance.EnablePlayersCreation();
        onFinishGame.AddListener(delegate ()
        {
            PlayersManager.Instance.HideAllHuds();
            PlayersManager.Instance.LockPlayersMovement();
            PlayersManager.Instance.DisablePlayersCreation();
            CameraFX.Instance.VerticalClap(() =>            {
                SceneManager.LoadScene("ResultScene");
            });
        });
        source = GetComponent<AudioSource>();
        source.Play();
        
    }
    void Update()
    {
        counterText.text = ((int)remainingTime/60).ToString() + ":" +((int)remainingTime % 60).ToString();
        remainingTime -= Time.deltaTime;
        if(remainingTime < 15.0f && !source.isPlaying)
        {
            source.clip = ShortGameTime;
            source.Play();
        }
        if(remainingTime < 0)
            onFinishGame.Invoke();
    }
}
