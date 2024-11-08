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
    void OnEnable()
    {
        remainingTime = gameTime;
        LeanTween.alphaCanvas(counterText.GetComponentInParent<CanvasGroup>(),1,1);
        onFinishGame.AddListener(delegate ()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
    void Update()
    {
        counterText.text = remainingTime.ToString();
        remainingTime -= Time.deltaTime;
        if(remainingTime < 0)
            onFinishGame.Invoke();
    }
}
