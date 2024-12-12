using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerCounter : MonoBehaviour
{
  
    private int players ;
    public Vector3 cameraDestination;
    public UnityEvent onPlayerEnter, onPlayerExit, onAllPlayersEnter;
    private void OnEnable()
    {
        players = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        players++;
        onPlayerEnter.Invoke();
        if (players == PlayersManager.Instance.players.Count)
        {
            onAllPlayersEnter.Invoke();
            Camera.main.gameObject.GetComponent<CameraTween>().MoveCameraTo(cameraDestination);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        players--;
        onPlayerExit.Invoke();
    }
}
