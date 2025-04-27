using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Items : MonoBehaviour
{
  
    //Object Creation
    private bool canCreate = true, fillingMana = true;
    private bool dontRepeat = true;
    public ObjectCreation objectGenerator;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;
    [SerializeField] SpriteRenderer itemHandSprite;
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
        SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadedSceneMode) { recievedObject = null; };
        onItemRecieved.AddListener(delegate (Sprite s) { itemHandSprite.sprite = s; });
        }
    private void OnDestroy()
    {
        GetComponent<PlayerInput>().actions.FindAction("GenerateMidObject").RemoveAllBindingOverrides();
    }
    public void SetItemHandSprite(Sprite s){itemHandSprite.sprite = s;}
    void CreateObject(InputAction.CallbackContext context, ObjectCreation.ObjectSizes size)
    {

        if (recievedObject != null)
        {
            GameObject instantiatedItem = recievedObject;
            InstantiateCreatedObject(instantiatedItem);
            itemHandSprite.sprite = null;
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
                spawnpos = Vector3.left * .55f;
            else
               spawnpos = Vector3.right * .55f;
            instantiatedItem = Instantiate(instantiatedItem, this.transform.position + (spawnpos*transform.localScale.x), Quaternion.Euler(0, 0, 0));
            instantiatedItem.GetComponent<Item>().creator = this.gameObject;
            instantiatedItem.GetComponent<Rigidbody2D>().AddForce(spawnpos * 60f, ForceMode2D.Impulse);
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