using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    private void Awake()
    {
        LeanTween.move(Camera.main.gameObject, Vector2.zero, 2).setOnComplete(delegate ()
        {
                PlayersManager.Instance.SetJoining(true);
        }).setEaseInOutElastic();  
    }
}
