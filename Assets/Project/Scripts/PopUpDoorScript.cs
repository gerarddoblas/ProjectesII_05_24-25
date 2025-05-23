using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpDoorScript : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    private bool isShowing = false;
    private int players = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            players++;
            if (!isShowing)
            {
                isShowing = true;
                LeanTween.scale(popUp, Vector3.one, .1f).setEaseInOutBounce();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<Player>())
        {
            players--;
            if (players <= 0)
            {
                isShowing = false;
                LeanTween.scale(popUp, Vector3.zero, .1f).setEaseInOutBounce();
            }
        }
    }
}
