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

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;

    public UnityEvent<Sprite> onItemRecieved;
    public UnityEvent onItemCreated;
    public GameObject recievedObject;
    public void EnableCreation() { canCreate = true; }
    public void DisableCreation() {  canCreate = false; }
    private void Awake()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();

        input.actions.FindAction("GenerateMidObject").started += CreateMidObject;
        

    }

    private void Start()
    {
    }
    private void Update()
    {
       /* if (mana < maxFill && fillingMana)
        {
            mana += fillspeed * Time.deltaTime;
            AudioManager.instance.PlayManaSound("ChargeMana");
            if (mana >= maxFill)
            {
                AudioManager.instance.manaSource.Stop();
                mana = maxFill;
            }
            onAlterMana.Invoke(this.mana);

        }*/
    }
    void CreateObject(InputAction.CallbackContext context, ObjectCreation.ObjectSizes size)
    {

        if (recievedObject != null)
        {
            GameObject instantiatedItem = recievedObject;
            InstantiateCreatedObject(instantiatedItem);
            recievedObject = null;
        }
    }
    void CreateMidObject(InputAction.CallbackContext context) => CreateObject(context, ObjectCreation.ObjectSizes.MEDIUM);

    void InstantiateCreatedObject(GameObject instantiatedItem)
    {
        AudioManager.instance.PlaySFX("CreationItem");
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
        onItemCreated.Invoke();
    }
    public void LockObjectCreation() { this.canCreate = false; }
    public void UnlockObjectCreation() { this.canCreate = true; }
    IEnumerator LockObjectCreation(float time)
    {
        this.LockObjectCreation();
        yield return new WaitForSeconds(time);
        this.UnlockObjectCreation();
        yield return null;
    }
}