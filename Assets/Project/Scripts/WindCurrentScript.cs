using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCurrentScript : MonoBehaviour
{
    [SerializeField] private bool hasTimer;
    [SerializeField] private float timeOn;
    [SerializeField] private float timeOff;
    private float curTime = 0f;
    private bool isOn = false;

    private ParticleSystem particle;
    private AreaEffector2D effector;
    // Start is called before the first frame update
    void Start()
    {
        particle = this.GetComponent<ParticleSystem>();
        effector = this.GetComponent<AreaEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasTimer) return;
        curTime -= Time.deltaTime;
        if(curTime <= 0f)
        {
            isOn = !isOn;
            curTime = (isOn) ? timeOn : timeOff;
            if (isOn) particle.Play(); else particle.Stop();
            effector.enabled = isOn;
        }
    }
}
