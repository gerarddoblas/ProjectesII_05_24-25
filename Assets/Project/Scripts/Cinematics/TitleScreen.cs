using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    Controls controls;

    [SerializeField] private GameObject title;
    [SerializeField] private TMP_Text pressSpaceText;
    [SerializeField] private TMP_Text startText;
    [SerializeField] private TMP_Text quitText;
    [SerializeField] private GameObject startDoor;
    [SerializeField] private GameObject quitDoor;

    private void Awake()
    {
        
        float scale = this.GetComponent<RectTransform>().localScale.x;
        float width = this.GetComponent<RectTransform>().rect.width;
        float height = this.GetComponent<RectTransform>().rect.height;

        

        LeanTween.move(Camera.main.gameObject, Vector2.zero, 7.5f).setOnUpdate(delegate(float r){
            if (Input.anyKeyDown)
            {
                AudioManager.instance.StopMusic();
                AudioManager.instance.PlayMusic("TitleScreen");
                LeanTween.cancel(Camera.main.gameObject);
                PlayersManager.Instance.SetJoining(true);
                CameraFX.Instance.Center2DCamera(.5f);
                LeanTween.move(title.gameObject, new Vector2(0, 2.75f), 1f).setEaseInOutBounce();

                //Press space
                LeanTween.moveLocal(pressSpaceText.gameObject, new Vector2(0, 4.45f), 1f).setEaseInOutBounce();

                //Start
                startText.transform.localPosition = new Vector2(startDoor.transform.position.x / scale, -height);
                LeanTween.moveLocal(startText.gameObject, new Vector3(0, 0.625f), 1f).setEaseInOutBounce();

                //Quit
                quitText.transform.localPosition = new Vector2(quitDoor.transform.position.x / scale, -height);
                LeanTween.moveLocal(quitText.gameObject, new Vector3(0, 0.625f), 1f).setEaseInOutBounce();
                
                LeanTween.value(1, 0, .5f).setOnUpdate(delegate (float r) { 
                    this.GetComponent<AudioSource>().volume = r;
                });
            }
        }).setOnComplete(delegate (){
            AudioManager.instance.PlayMusic("TitleScreen");
            PlayersManager.Instance.SetJoining(true);
            LeanTween.move(title.gameObject, new Vector2(0, 2.75f), 1f).setEaseInOutBounce();

            //Press space
            LeanTween.moveLocal(pressSpaceText.gameObject, new Vector2(0, 4.45f), 1f).setEaseInOutBounce();

            //Start
            startText.transform.localPosition = new Vector2(startDoor.transform.position.x / scale, -height);
            LeanTween.moveLocal(startText.gameObject, new Vector3(0, 0.625f), 1f).setEaseInOutBounce();

            //Quit
            quitText.transform.localPosition = new Vector2(quitDoor.transform.position.x / scale, -height);
            LeanTween.moveLocal(quitText.gameObject, new Vector3(0, 0.625f), 1f).setEaseInOutBounce();
        }).setEaseInOutBounce();
    }

    private void Start()
    {
        AudioManager.instance.PlayMusic("opening");
    }
}
