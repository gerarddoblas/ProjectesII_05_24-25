using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    Controls controls;
    private void Awake()
    {
        LeanTween.move(Camera.main.gameObject, Vector2.zero, 7.5f).setOnComplete(delegate ()
        {
                PlayersManager.Instance.SetJoining(true);
        }).setEaseInOutBounce();
        
    }
}