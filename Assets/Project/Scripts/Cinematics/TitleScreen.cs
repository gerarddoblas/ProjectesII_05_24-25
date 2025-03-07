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

        RectTransform startTextRect = startText.GetComponent<RectTransform>();
        RectTransform quitTextRect = quitText.GetComponent<RectTransform>();

        LeanTween.move(Camera.main.gameObject, Vector2.zero, 7.5f).setOnUpdate(delegate(float r){
            if (Input.anyKeyDown)
            {
                LeanTween.cancel(Camera.main.gameObject);
                PlayersManager.Instance.SetJoining(true);
                CameraFX.Instance.Center2DCamera(.5f);
                LeanTween.move(title.gameObject, new Vector2(0, 2.75f), 1f).setEaseInOutBounce();

                //Press space
                LeanTween.move(pressSpaceText.GetComponent<RectTransform>(), new Vector2(0, 20), 1f).setEaseInOutBounce();

                //Start
                startTextRect.anchorMin = new Vector2(0.5f, 0.5f);
                startTextRect.anchorMax = new Vector2(0.5f, 0.5f);
                startTextRect.localPosition = new Vector2(startDoor.transform.position.x / scale, -height);
                LeanTween.move(startTextRect, (startDoor.transform.position + new Vector3(0, 2, 0)) / scale, 1f).setEaseInOutBounce();
                
                //Quit
                quitTextRect.anchorMin = new Vector2(0.5f, 0.5f);
                quitTextRect.anchorMax = new Vector2(0.5f, 0.5f);
                quitTextRect.localPosition = new Vector2(quitDoor.transform.position.x / scale, -height);
                LeanTween.move(quitTextRect, (quitDoor.transform.position + new Vector3(0, 2, 0)) / scale, 1f).setEaseInOutBounce();
                
                LeanTween.value(1, 0, .5f).setOnUpdate(delegate (float r) { 
                    this.GetComponent<AudioSource>().volume = r;
                });
            }
        }).setOnComplete(delegate (){
            PlayersManager.Instance.SetJoining(true);
            LeanTween.move(title.gameObject, new Vector2(0, 2.75f), 1f).setEaseInOutBounce();

            //Press space
            LeanTween.move(pressSpaceText.GetComponent<RectTransform>(), new Vector2(0, 20), 1f).setEaseInOutBounce();

            //Start
            startTextRect.anchorMin = new Vector2(0.5f, 0.5f);
            startTextRect.anchorMax = new Vector2(0.5f, 0.5f);
            startTextRect.localPosition = new Vector2(startDoor.transform.position.x / scale, -height);
            LeanTween.move(startTextRect, (startDoor.transform.position + new Vector3(0, 2, 0)) / scale, 1f).setEaseInOutBounce();
            
            //Quit
            quitTextRect.anchorMin = new Vector2(0.5f, 0.5f);
            quitTextRect.anchorMax = new Vector2(0.5f, 0.5f);
            quitTextRect.localPosition = new Vector2(quitDoor.transform.position.x / scale, -height);
            LeanTween.move(quitTextRect, (quitDoor.transform.position + new Vector3(0, 2, 0)) / scale, 1f).setEaseInOutBounce();
        }).setEaseInOutBounce();
        
    }
}
