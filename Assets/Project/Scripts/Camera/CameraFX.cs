using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFX : MonoBehaviour
{
    public static CameraFX Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    //MoveFx
    public void CenterCamera() { 
        LeanTween.cancel(Camera.main.transform.gameObject);
        Camera.main.transform.position = Vector3.zero; 
    }
    public void Center2DCamera(float time) { 
        LeanTween.cancel(Camera.main.transform.gameObject);
        Camera.main.transform.LeanMove(new Vector3(0, 0, Camera.main.transform.position.z), time); 
    }
    public void Center2DCamera()
    {
        LeanTween.cancel(Camera.main.transform.gameObject);
        Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
    }
    public void CenterCamera(float time)
    {
        LeanTween.cancel(Camera.main.transform.gameObject);
        Camera.main.transform.LeanMove(new Vector3(0, 0, 0), time);
    }
    public void MoveCamera(Vector2 destPos) { MoveCamera(new Vector3(destPos.x, destPos.y, Camera.main.transform.position.z)); }
    public void MoveCamera(Vector3 destPos) {
        LeanTween.cancel(Camera.main.transform.gameObject);
        Camera.main.transform.position = destPos; 
    }

    public void MoveCamera(Vector2 destPos, float time){MoveCamera(new Vector3(destPos.x, destPos.y, Camera.main.transform.position.z), time);}
    public void MoveCamera(Vector3 destPos,float time) { 
        LeanTween.cancel(Camera.main.transform.gameObject);
        Camera.main.transform.LeanMove(destPos, time); 
    }
    //Transitions TODO



}
