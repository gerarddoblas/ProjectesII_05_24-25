using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crown : MonoBehaviour
{
   [SerializeField] Player owner;
   [SerializeField] bool canBepicked = true;
   [SerializeField] public void SetCanBePicked(bool newValue) { canBepicked = newValue; }
    [SerializeField]private Rigidbody2D rb;
   [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField]public static Crown Instance { get; private set; }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadedSceneMode)
        {
            try
            {
                if (GameController.Instance.currentGameMode.GetType() == typeof(StealTheCrown))
                    this.transform.position = Vector3.zero;
                else
                    this.transform.position = new Vector3(-1000, 0);
            } catch { Debug.LogWarning("Object not instantiated"); }
        };
    }
    private void Update()
    {
        if(owner)
            this.transform.position = owner.positionMarker.transform.position;
    }
    public Player GetOwner()
    {
        return owner != null ? owner : null;
    }
    public void RemoveOwner() {  
        owner = null; 
        boxCollider.enabled = true;
        rb.simulated = true;
        
    }
    IEnumerator AvoidPicking()
    {   
        canBepicked = false;
        yield return new WaitForSeconds(3);
        canBepicked = true;
        yield return null;
    }
    public void SetOwner(Player newOwner)
    {
        owner = newOwner;
        boxCollider.enabled = false;
        rb.simulated = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent(out Player p))
            SetOwner(p);
    }
}
