using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayersManager : MonoBehaviour
{
    public Color[] playerColours;
    public GameObject canvasPrefab;
    public List<GameObject> players;
    public List<GameObject> playersCanvas;
    PlayerInputManager playerInputManager;
    public GameObject playerContainer, hudsContainer;
    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoin;
        playerInputManager.onPlayerLeft += OnPlayerLeft;
    }
    private void OnPlayerJoin(PlayerInput input)
    {
        Debug.Log("hi");
        input.gameObject.transform.SetParent(playerContainer.transform);     
        input.gameObject.GetComponentInChildren<SpriteRenderer>().color = playerColours[players.Count];
        players.Add(input.gameObject);
        GameObject instantiatedHUD = GameObject.Instantiate(canvasPrefab,hudsContainer.transform);
        playersCanvas.Add(instantiatedHUD);
        instantiatedHUD.GetComponent<PlayerHud>().backgroundImage.GetComponent<Image>().color = playerColours[players.Count];
        //Sobreescribir eventos de player
    }
    private void OnPlayerLeft(PlayerInput input)
    {
        for (int i = 0; i < playerInputManager.playerCount; i++) {
            if (input.gameObject == playerContainer.transform.GetChild(i)) {
                Destroy(playerContainer.transform.GetChild(i));
                Destroy(hudsContainer.transform.GetChild(i));
            } 
        }
    }
}
