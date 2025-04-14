using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraFX : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public Image instructions;
    public bool clapOnEnable = false;
    public static CameraFX Instance { get; private set; }
    Vector3 originalCamPos;
    GameObject topClap, bottomClap;
    [SerializeField]float timeBeforeFirstClap;
    float cameraSize;
    [SerializeField] bool showInstructions = true;
    public void SetClap()
    {
        topClap.transform.localPosition = new Vector3(0, Camera.main.orthographicSize, 1); 
        bottomClap.transform.localPosition = new Vector3(0, -Camera.main.orthographicSize, 1);
    }
    public void UnsetClap()
    {
        topClap.transform.localPosition = new Vector3(0, Camera.main.orthographicSize * 2, 1);
        bottomClap.transform.localPosition = new Vector3(0, -Camera.main.orthographicSize * 2, 1);
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
        originalCamPos = this.transform.position;
        //TopClapSettings
        topClap = this.transform.GetChild(0).gameObject;
        topClap.transform.localPosition = new Vector3(0, Camera.main.orthographicSize*2, 1);
        topClap.transform.localScale = new Vector3(
            (Camera.main.orthographicSize * 2)* Camera.main.aspect,
            Camera.main.orthographicSize * 2
            ,1
         );
        //BottomClapSettings
        bottomClap = this.transform.GetChild(1).gameObject;
        bottomClap.transform.localPosition = new Vector3(0, -Camera.main.orthographicSize * 2, 1);
        bottomClap.transform.localScale = new Vector3(
            (Camera.main.orthographicSize * 2) * Camera.main.aspect,
            Camera.main.orthographicSize * 2
            , 1
         );
        if (clapOnEnable)
            ReverseVerticalClap(() => { Debug.Log("Test clap finished"); });
    }
    

    //MoveFx
    
    public void CenterCamera() { 
        LeanTween.cancel(Camera.main.transform.gameObject);
        Camera.main.transform.position = Vector3.zero; 
    }
    public void Center2DCamera(float time) { 
        LeanTween.cancel(Camera.main.transform.gameObject);
        Camera.main.transform.LeanMove(new Vector3(0, 0, Camera.main.transform.position.z), time).setOnUpdate(delegate (Vector3 updatedpos)
        {
            originalCamPos = updatedpos;
            Camera.main.transform.position = updatedpos;
        }); ;
    }
    public void Center2DCamera()
    {
        LeanTween.cancel(Camera.main.transform.gameObject);
        Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
    }
    public void CenterCamera(float time)
    {
        LeanTween.cancel(Camera.main.transform.gameObject);
        Camera.main.transform.LeanMove(new Vector3(0, 0, 0), time).setOnUpdate(delegate (Vector3 updatedpos)
        {
            originalCamPos = updatedpos;
            Camera.main.transform.position = updatedpos;
        }); ;
    }
    public void MoveCamera(Vector2 destPos) { MoveCamera(new Vector3(destPos.x, destPos.y, Camera.main.transform.position.z)); }
    public void MoveCamera(Vector3 destPos) {
        LeanTween.cancel(Camera.main.transform.gameObject);
        Camera.main.transform.position = destPos; 
    }

    public void MoveCamera(Vector2 destPos, float time){MoveCamera(new Vector3(destPos.x, destPos.y, Camera.main.transform.position.z), time);}
    public void MoveCamera(Vector3 destPos,float time) { 
        LeanTween.cancel(Camera.main.transform.gameObject);
        Camera.main.transform.LeanMove(destPos, time).setOnUpdate(delegate (Vector3 updatedpos)
        {
            originalCamPos = updatedpos;
            Camera.main.transform.position = updatedpos;
        }); 
    }
    //CameraShake
    public void CameraShake(float time, float magnitude){StartCoroutine(StartShake(time, magnitude));}
    private IEnumerator StartShake(float time, float magnitude)
    {
        originalCamPos = Camera.main.transform.position;
        while(time > 0)
        {
            Camera.main.transform.position = originalCamPos + new Vector3(UnityEngine.Random.Range(-magnitude,magnitude), UnityEngine.Random.Range(-magnitude, magnitude), UnityEngine.Random.Range(-magnitude, magnitude));
            time-= Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.position = originalCamPos;
        yield return null;
    }
    //Transitions TODO
    public void VerticalClap(){ VerticalClap(1, null); }
    public void VerticalClap(float time) {VerticalClap(time, null); }
    public void ReverseVerticalClap() { ReverseVerticalClap(1,null); }
    public void ReverseVerticalClap(float time) { ReverseVerticalClap(time, null);    }

    public void VerticalClap(Action onComplete) { VerticalClap(1,onComplete); }
    public void VerticalClap(float time, Action onComplete){VerticalClap(null,time,onComplete);}
    public void ReverseVerticalClap(Action onComplete) { ReverseVerticalClap(1,onComplete); }
    public void ReverseVerticalClap(float time, Action onComplete){ReverseVerticalClap(null, time, onComplete);}
    public void VerticalClap(Action onBegin,float time, Action onComplete)
    {

        topClap.transform.localPosition = new Vector3(0, Camera.main.orthographicSize * 2, 1);
        LeanTween.moveLocal(topClap, new Vector3(0, Camera.main.orthographicSize, 1), time).setOnStart(()=> { onBegin?.Invoke(); });
        bottomClap.transform.localPosition = new Vector3(0, -Camera.main.orthographicSize * 2, 1);
        LeanTween.moveLocal(bottomClap, new Vector3(0, -Camera.main.orthographicSize, 1), time).setOnComplete(() => { onComplete?.Invoke(); });
    }
    public void ReverseVerticalClap(Action onBegin,float time, Action onComplete)
    {
        topClap.transform.localPosition = new Vector3(0, Camera.main.orthographicSize, 1);
        LeanTween.moveLocal(topClap, new Vector3(0, Camera.main.orthographicSize * 2, 1), time).setOnStart(()=>{onBegin?.Invoke();});
        bottomClap.transform.localPosition = new Vector3(0, -Camera.main.orthographicSize, 1);
        LeanTween.moveLocal(bottomClap, new Vector3(0, -Camera.main.orthographicSize * 2, 1), time).setOnComplete(() => { onComplete?.Invoke(); });
    }
}
