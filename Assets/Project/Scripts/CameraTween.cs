using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTween : MonoBehaviour
{
    public void MoveCameraTo(Vector3 destination)
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.move(this.gameObject, destination, 1);
    }
    public void MoveCameraTo(Vector3 destination, float time)
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.move(this.gameObject, destination, time);
    }
}
