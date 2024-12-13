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
    private bool canCreate = true;
    public ObjectCreation objectGenerator;
    public GameObject smallObject, mediumObject, bigObject;
    public UnityEvent<Sprite> onGenerateRandomSmallObject, onGenerateRandomMidObject, onGenerateRandomBigObject;
    Coroutine charging;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody;
    public void EnableCreation() { canCreate = true; }
    public void DisableCreation() {  canCreate = false; }
    private void Awake()
    {
        mana = 0;
        fillspeed = 0.5f;
        maxFill = 3;
        PlayerInput input = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        input.actions.FindAction("GenerateSmallObject").started += CreateSmallObject;
        input.actions.FindAction("GenerateMidObject").started += CreateMidObject;
        input.actions.FindAction("GenerateBigObject").started += CreateBigObject;
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
    void CreateSmallObject(InputAction.CallbackContext context)
    {
        if(mana >= 1f && canCreate)
        {
            mana -= 1f;
            GameObject  instantiatedItem = smallObject;
            smallObject = objectGenerator.GetRandomSmallObject();
            onGenerateRandomSmallObject.Invoke(smallObject.GetComponent<SpriteRenderer>().sprite);
            InstantiateCreatedObject(instantiatedItem);
        }
    }
    void CreateMidObject(InputAction.CallbackContext context)
    {
        if (mana >= 2f && canCreate)
        {
            mana -= 2f;
            GameObject instantiatedItem = mediumObject;
            mediumObject = objectGenerator.GetRandomMediumObject();
            onGenerateRandomMidObject.Invoke(mediumObject.GetComponent<SpriteRenderer>().sprite);
            InstantiateCreatedObject(instantiatedItem);
        }
    }
    void CreateBigObject(InputAction.CallbackContext context)
    {
        if (mana >= 3f && canCreate)
        {
            mana -= 3f;
            GameObject instantiatedItem = bigObject;
            bigObject = objectGenerator.GetRandomBigObject();
            onGenerateRandomBigObject.Invoke(bigObject.GetComponent<SpriteRenderer>().sprite);
            InstantiateCreatedObject(instantiatedItem);
        }
    }
    void InstantiateCreatedObject(GameObject instantiatedItem)
    {
        if (instantiatedItem.GetComponent<Item>().consumible)
            StartCoroutine(instantiatedItem.GetComponent<Item>().Effect(this.gameObject));
        else
        {
            Vector3 spawnpos = Vector3.zero;
            if (spriteRenderer.flipX)
                spawnpos = Vector3.right;
            else
                spawnpos = Vector3.left;
            instantiatedItem = Instantiate(instantiatedItem, this.transform.position + spawnpos, Quaternion.Euler(0, 0, 0));
            instantiatedItem.GetComponent<Item>().creator = this.gameObject;
            instantiatedItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(spawnpos.x * 10, 0), ForceMode2D.Impulse);
        }
        onAlterMana.Invoke(this.mana);
    }
}