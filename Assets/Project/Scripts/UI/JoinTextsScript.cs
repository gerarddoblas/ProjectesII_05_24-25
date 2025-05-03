using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinTextsScript : MonoBehaviour
{
    public TMP_Text[] texts = new TMP_Text[4];
    // Start is called before the first frame update
    void Start()
    {
        foreach(var text in texts) LeanTween.scale(text.gameObject, Vector3.one * .7f, 1f).setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
