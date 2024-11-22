using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class playerCounter : MonoBehaviour
{
    public PlayerInputManager playerInputManager;
    private int players ;
    public UnityEvent onPlayerEnter, onPlayerExit, onAllPlayersEnter;
    private void OnEnable()
    {
        players = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        players++;
        onPlayerEnter.Invoke();
       // if (players == playerInputManager.playerCount)
        //{
            onAllPlayersEnter.Invoke();
            Camera.main.gameObject.GetComponent<CameraTween>().MoveCameraTo(new Vector3(0, -7.5f, -10));
        //}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        players--;
        onPlayerExit.Invoke();
    }
}
