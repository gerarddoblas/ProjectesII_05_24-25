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
    private bool canCreate = true, fillingMana = true;
    private bool dontRepeat = true;
    public ObjectCreation objectGenerator;

    public UnityEvent<Sprite> onGenerateRandomSmallObject, onGenerateRandomMidObject, onGenerateRandomBigObject;
    private Coroutine charging;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;

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
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
            ItemHud.Instance.objects[i] = objectGenerator.GetRandomObject((ObjectCreation.ObjectSizes)i);
        onGenerateRandomSmallObject.Invoke(ItemHud.Instance.objects[0].GetComponent<SpriteRenderer>().sprite);
        onGenerateRandomMidObject.Invoke(ItemHud.Instance.objects[1].GetComponent<SpriteRenderer>().sprite);
        onGenerateRandomBigObject.Invoke(ItemHud.Instance.objects[2].GetComponent<SpriteRenderer>().sprite);
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
    void CreateObject(InputAction.CallbackContext context, ObjectCreation.ObjectSizes size)
    {
        if (mana < (int)size || !canCreate) return;
        mana -= (int)size;
        onAlterMana.Invoke(this.mana);

        GameObject instantiatedItem = ItemHud.Instance.objects[(int)size];
        ItemHud.Instance.objects[(int)size] = objectGenerator.GetRandomObject(size);
        while (instantiatedItem.GetComponent<SpriteRenderer>().sprite == ItemHud.Instance.objects[(int)size].GetComponent<SpriteRenderer>().sprite && dontRepeat)
            ItemHud.Instance.objects[(int)size] = objectGenerator.GetRandomSmallObject();

        switch(size)
        {
            case ObjectCreation.ObjectSizes.SMALL:
                onGenerateRandomSmallObject.Invoke(ItemHud.Instance.objects[(int)size].GetComponent<SpriteRenderer>().sprite);
                break;
            case ObjectCreation.ObjectSizes.MEDIUM:
                onGenerateRandomMidObject.Invoke(ItemHud.Instance.objects[(int)size].GetComponent<SpriteRenderer>().sprite);
                break;
            case ObjectCreation.ObjectSizes.LARGE:
                onGenerateRandomBigObject.Invoke(ItemHud.Instance.objects[(int)size].GetComponent<SpriteRenderer>().sprite);
                break;
        }
        InstantiateCreatedObject(instantiatedItem);
    }
    void CreateSmallObject(InputAction.CallbackContext context) => CreateObject(context, ObjectCreation.ObjectSizes.SMALL);
    void CreateMidObject(InputAction.CallbackContext context) => CreateObject(context, ObjectCreation.ObjectSizes.MEDIUM);
    void CreateBigObject(InputAction.CallbackContext context) => CreateObject(context, ObjectCreation.ObjectSizes.LARGE);

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