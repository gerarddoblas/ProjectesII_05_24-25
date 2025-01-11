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
    private bool canCreate = true,fillingMana = true;
    private bool dontRepeat = true;
    private GameObject lastSmallObject, lastMidObject, lastBigObject;
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
        fillspeed = 1;
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
        if (mana < maxFill && fillingMana)
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
            while(instantiatedItem.GetComponent<SpriteRenderer>().sprite == smallObject.GetComponent<SpriteRenderer>().sprite && dontRepeat)
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
            while (instantiatedItem.GetComponent<SpriteRenderer>().sprite == mediumObject.GetComponent<SpriteRenderer>().sprite && dontRepeat)
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
            while (instantiatedItem.GetComponent<SpriteRenderer>().sprite == bigObject.GetComponent<SpriteRenderer>().sprite && dontRepeat)
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
                spawnpos = Vector3.left * 2f;
            else
               spawnpos = Vector3.right * 2f;
            instantiatedItem = Instantiate(instantiatedItem, this.transform.position + spawnpos, Quaternion.Euler(0, 0, 0));
            instantiatedItem.GetComponent<Item>().creator = this.gameObject;
            instantiatedItem.GetComponent<Rigidbody2D>().AddForce(spawnpos * 12.5f, ForceMode2D.Impulse);
        }
        onAlterMana.Invoke(this.mana);
    }
    void LockObjectCreation() { this.canCreate = false; }
    void UnlockObjectCreation() { this.canCreate = true; }
    IEnumerator LockObjectCreation(float time)
    {
        this.LockObjectCreation();
        yield return new WaitForSeconds(time);
        this.UnlockObjectCreation();
        yield return null;
    }
    void LockManaFill() { this.fillingMana = false; }
    void UnlockManaFill() { this.fillingMana = true; }
    IEnumerator LockManaFill(float time)
    {
        this.LockManaFill();
        yield return new WaitForSeconds(time);
        this.UnlockManaFill();
        yield return null;
    }
    public void LockManaAndCreation() { 
        this.fillingMana = false; 
        this.canCreate = false;
    }
    public void UnlockManaAndCreation() {
        this.fillingMana = true;
        this.canCreate = true;
    }
    IEnumerator LockManaAndCreation(float time)
    {
        this.LockManaAndCreation();
        yield return new WaitForSeconds(time);
        this.UnlockManaAndCreation();
        yield return null;
    }
}