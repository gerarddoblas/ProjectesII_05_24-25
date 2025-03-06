using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    Controls controls;
    private void Awake()
    {
        LeanTween.move(Camera.main.gameObject, Vector2.zero, 7.5f).setOnUpdate(delegate(float r){
            if (Input.anyKeyDown)
            {
                LeanTween.cancel(Camera.main.gameObject);
                PlayersManager.Instance.SetJoining(true);
                CameraFX.Instance.Center2DCamera(.5f);
                LeanTween.move(this.gameObject, new Vector2(0, 2.75f), 1f).setEaseInOutBounce();
                LeanTween.value(1, 0, .5f).setOnUpdate(delegate (float r) { 
                    this.GetComponent<AudioSource>().volume = r;
                });
            }
        }).setOnComplete(delegate (){
            PlayersManager.Instance.SetJoining(true);
            LeanTween.move(this.gameObject, new Vector2(0, 2.75f), 1f).setEaseInOutBounce();
        }).setEaseInOutBounce();
        
    }
}
