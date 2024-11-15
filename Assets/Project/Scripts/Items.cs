using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Items : MonoBehaviour
{
    //Mana
    public float mana, fillspeed, maxFill;
    public float consumedMana;
    public UnityEvent<float> onAlterMana;
    //Object Creation
    public ObjectCreation objectGenerator;
    public GameObject smallObject, mediumObject, bigObject;
    public UnityEvent<Sprite> onGenerateRandomSmallObject, onGenerateRandomMidObject, onGenerateRandomBigObject;
    Coroutine charging;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody;

    private void Awake()
    {
        mana = 0;
        fillspeed = 0.5f;
        maxFill = 3;
        PlayerInput input = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        input.actions.FindAction("Attack").started += StartCreating;
        input.actions.FindAction("Attack").canceled += Create;
        smallObject = objectGenerator.GetRandomSmallObject();
        onGenerateRandomSmallObject.Invoke(smallObject.GetComponent<SpriteRenderer>().sprite);
        mediumObject = objectGenerator.GetRandomMediumObject();
        onGenerateRandomMidObject.Invoke(mediumObject.GetComponent<SpriteRenderer>().sprite);
        bigObject = objectGenerator.GetRandomBigObject();
        onGenerateRandomBigObject.Invoke(bigObject.GetComponent<SpriteRenderer>().sprite);
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
            //Debug.Log("Consuming Mana");
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
        //Debug.Log("Creating...");
        GameObject instantiatedItem = null;
        switch ((int)consumedMana)
        {
            case 1:
                instantiatedItem = smallObject;
                smallObject = objectGenerator.GetRandomSmallObject();
                onGenerateRandomSmallObject.Invoke(smallObject.GetComponent<SpriteRenderer>().sprite);
                //Debug.Log("generating small object");
                break;
            case 2:
                instantiatedItem = mediumObject;
                mediumObject = objectGenerator.GetRandomMediumObject();
                onGenerateRandomMidObject.Invoke(mediumObject.GetComponent<SpriteRenderer>().sprite);
                //Debug.Log("generating mid object");
                break;
            case 3:
                instantiatedItem = bigObject;
                bigObject = objectGenerator.GetRandomBigObject();
                onGenerateRandomBigObject.Invoke(bigObject.GetComponent<SpriteRenderer>().sprite);
                //Debug.Log("generating big object");
                break;
        }
        if (instantiatedItem!=null)
        {
            if (instantiatedItem.GetComponent<Item>().consumible)
                instantiatedItem.GetComponent<Item>().Effect(this.gameObject);
            else
            {
                Vector3 spawnpos = Vector3.zero;
                if (spriteRenderer.flipX)
                    spawnpos = Vector3.right;
                else
                    spawnpos = Vector3.left;
                    instantiatedItem = Instantiate(instantiatedItem, this.transform.position + spawnpos, Quaternion.Euler(0, 0, 0));
                instantiatedItem.GetComponent <Item>().creator = this.gameObject;
                instantiatedItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(spawnpos.x*10, 0), ForceMode2D.Impulse); 
            }
        }
        mana -= (int)consumedMana;
        onAlterMana.Invoke(this.mana);
        consumedMana = 0;
    }
}