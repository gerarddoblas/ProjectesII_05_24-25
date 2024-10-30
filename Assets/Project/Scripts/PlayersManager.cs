using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManager : MonoBehaviour
{
   PlayerInputManager playerInputManager;
    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoin;
    }
    private void OnPlayerJoin(PlayerInput input)
    {
        Debug.Log("hi");
        input.gameObject.transform.SetParent(this.transform);      
    }
}
