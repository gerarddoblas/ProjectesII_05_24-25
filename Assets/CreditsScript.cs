using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScript : MonoBehaviour
{
    [SerializeField] private GameObject credits;
    // Start is called before the first frame update
    void Start()
    {
        credits.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision) { if (collision.GetComponent<Player>()) credits.SetActive(true); }
    private void OnTriggerExit2D(Collider2D collision) { if (collision.GetComponent<Player>()) credits.SetActive(false); }
}
