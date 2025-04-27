using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crown : MonoBehaviour
{
   [SerializeField] Player owner;
    private Player lastOwner = null;
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


        if (GameController.Instance.currentGameMode.GetType().Equals(typeof(StealTheCrown)))
            this.transform.position = Vector3.zero;
        else
            this.gameObject.SetActive(false);
     
    }
    private void Update()
    {
        if(owner)
            this.transform.position = owner.positionMarker.transform.position;
    }
    public Player GetOwner()
    {
        return owner;
    }
    public void RemoveOwner() {
        lastOwner = owner;
        owner = null; 
        boxCollider.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.simulated = true;
        rb.AddForce(new Vector2(UnityEngine.Random.Range(-100, 100), 500));
        StartCoroutine(AvoidPicking());
    }
    IEnumerator AvoidPicking()
    {   
        canBepicked = false;
        yield return new WaitForSeconds(1.5f);
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
        if (collision.transform.TryGetComponent(out Player p))
        {
            if (lastOwner != null && p == lastOwner)
            {
                if (canBepicked)
                    SetOwner(p);
            }
            else
                SetOwner(p);
        }
    }
}
