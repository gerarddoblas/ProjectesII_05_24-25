using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : TriggerEffector
{
    public GameObject mainCamera;
    public GameObject[] objetsToEnable;
    public GameObject[] objetsToDisable;
    public Vector3 cameraDestination;
    public float timeToMove;
    void Start()
    {
        mainCamera = Camera.main.gameObject;
        onEntry.AddListener(delegate (){
            LeanTween.move(mainCamera, cameraDestination, timeToMove);
        });
    }

}
