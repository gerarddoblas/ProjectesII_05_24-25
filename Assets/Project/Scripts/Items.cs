using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Items : MonoBehaviour
{
    public float mana, fillspeed, maxFill;
    public float consumedMana;
    public ObjectCreation objectGenerator;

    public UnityEvent<float> onAlterMana;
    Coroutine charging;

    private void Awake()
    {
        mana = 0;
        fillspeed = 0.5f;
        maxFill = 3;
        PlayerInput input = GetComponent<PlayerInput>();
        input.actions.FindAction("Attack").started += StartCreating;
        input.actions.FindAction("Attack").canceled += Create;
    }

    private void Update()
    {
        if (mana < maxFill)
        {
            mana += fillspeed * Time.deltaTime;
            if (mana >= maxFill)
                mana = maxFill;
            onAlterMana.Invoke(this.mana);
        }
    }
    void StartCreating(InputAction.CallbackContext context)
    {
        if (mana >= 1)
            consumedMana = 1;
        charging = StartCoroutine(CharginCreation(context));
    }
    IEnumerator CharginCreation(InputAction.CallbackContext context)
    {
        while (true)
        {
            Debug.Log("Consuming Mana");
            if (consumedMana < mana)
            {
                consumedMana += (fillspeed * Time.deltaTime * 4);
                if (consumedMana > mana)
                    consumedMana = mana;
            }
            yield return null;
        }
    }
    void Create(InputAction.CallbackContext context)
    {
        StopCoroutine(charging);
        charging = null;
        Debug.Log("Creating...");
        GameObject instantiatedItem = null;
        switch ((int)consumedMana)
        {
            case 1:
                instantiatedItem =  objectGenerator.GetRandomSmallObject();
                //Debug.Log("generating small object");
                break;
            case 2:
                instantiatedItem =  objectGenerator.GetRandomMediumObject();
                //Debug.Log("generating mid object");
                break;
            case 3:
                instantiatedItem = objectGenerator.GetRandomBigObject();
                //Debug.Log("generating big object");
                break;
        }
        if (instantiatedItem)
        {
            Instantiate(instantiatedItem, this.transform.position, Quaternion.Euler(0,0,0));
        }
        mana -= (int)consumedMana;
        onAlterMana.Invoke(this.mana);
        consumedMana = 0;
    }
}