using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    Controls controls;

    [SerializeField] private GameObject title;
    [SerializeField] private TMP_Text pressSpaceText;
    [SerializeField] private TMP_Text startText;
    [SerializeField] private TMP_Text quitText;
    [SerializeField] private TMP_Text creditsText;
    [SerializeField] private TMP_Text controlsText;

    private void Awake()
    {
        LeanTween.move(Camera.main.gameObject, Vector2.zero, 7.5f).setOnUpdate(delegate(float r){
            if (Input.anyKeyDown)
            {
                AudioManager.instance.StopMusic();

                LeanTween.cancel(Camera.main.gameObject);
                CameraFX.Instance.Center2DCamera(.5f);

                Animation();
            }
        }).setOnComplete(delegate (){ Animation(); }).setEaseInOutBounce();
    }

    private void Animation()
    {
        AudioManager.instance.PlayMusic("TitleScreen");
        PlayersManager.Instance.SetJoining(true);

        //Move
        LeanTween.move(title, endPos(title), 1f).setEaseInOutBounce().setOnComplete(delegate () { LeanTween.rotate(title, new Vector3(0, 0, 10), .1f).setEaseInOutBounce(); });
        LeanTween.moveLocal(pressSpaceText.gameObject, new Vector2(0, 4.45f), 1f).setEaseInOutBounce();
        LeanTween.moveLocal(startText.gameObject, endPos(startText.gameObject), 1f).setEaseInOutBounce();
        LeanTween.moveLocal(quitText.gameObject, endPos(quitText.gameObject), 1f).setEaseInOutBounce();
        LeanTween.moveLocal(creditsText.gameObject, endPos(quitText.gameObject), 1f).setEaseInOutBounce();
        LeanTween.moveLocal(controlsText.gameObject, endPos(quitText.gameObject), 1f).setEaseInOutBounce();
    }

    private void Start()
    {
        AudioManager.instance.PlayMusic("opening");
    }

    private Vector3 endPos(GameObject go) => (Vector3)Variables.Object(go).Get("endPos");
}
