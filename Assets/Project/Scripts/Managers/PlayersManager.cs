using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

[Serializable]public struct PlayerHUDPosPreset
{
    public Vector2 anchorMin, anchorMax, position;
}
public class PlayersManager : MonoBehaviour
{
    public static PlayersManager Instance { get; private set; }

    [Header("Joinig options")]
    [SerializeField] bool enabledHUDByDefault, enableCreationByDefault;
    public PlayerInputManager playerInputManager;

    [Header("Object references")]
    [SerializeField] private GameObject playerContainer, hudsContainer, colourpicker;
    public GameObject GetColourPicker() { return colourpicker; }
    [Header("Lists")]
    public List<GameObject> players;
    public List<GameObject> playersCanvas;
    public List<Vector3Int> playerSpawnPositions;

    [Header("Visual Parameters")]
    [SerializeField] private GameObject canvasPrefab;
    [SerializeField] PlayerHUDPosPreset[] playerHUDPosPresets;

    public JoinTextsScript joinTextsScript;
    public UnityEvent onAnyActionPerformed;

    public void HealAllPlayers()
    {
        foreach (GameObject player in players)
            player.GetComponent<HealthBehaviour>().FullHeal();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }
    public void StopPlayers()
    {
        for (int i = 0; i < players.Count; ++i)
            players[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
    public void SetPlayersPosition() {
        hudsContainer.transform.position = Vector2.zero;
        if (playerSpawnPositions.Count != 4)
            for (int i = 0; i < players.Count; ++i)
                players[i].transform.position = new Vector3Int(0, 0, 0);
        else
            for (int i = 0; i < players.Count; ++i)
                players[i].transform.position = playerSpawnPositions[i];
        //onAnyActionPerformed.RemoveAllListeners();
    }

    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoin;
               
    }

    private void OnPlayerJoin(PlayerInput input)
    {
        GameObject player = input.gameObject;

        player.transform.SetParent(playerContainer.transform);

        int slotIndex = players.FindIndex(p => p == null);

        if (slotIndex == -1)
        {
            slotIndex = players.Count;
            players.Add(null);
            playersCanvas.Add(null);
        }

        players[slotIndex] = player;


        GameObject instantiatedHUD = Instantiate(canvasPrefab, hudsContainer.transform);
        playersCanvas[slotIndex] = instantiatedHUD;
        joinTextsScript?.texts[slotIndex].gameObject.SetActive(false);

        var instanceScript = instantiatedHUD.GetComponent<PlayerHud>();
        //instanceScript.SetColour(playerColours[slotIndex]);

        player.GetComponent<Items>().onItemRecieved.AddListener(delegate (Sprite s) {
            instanceScript.SetItemSprite(s);
        });

        player.GetComponent<Items>().onItemCreated.AddListener(delegate () {
            instanceScript.ClearItemSprite();
        });

        player.GetComponent<HealthBehaviour>().OnAlterHealth.AddListener(delegate (int mh, int h) {
            instanceScript.SetHealthbar(mh, h);
        });

        if (!enabledHUDByDefault)
            instantiatedHUD.SetActive(false);
        else
            instantiatedHUD.SetActive(true);

        RectTransform hudRect = instantiatedHUD.GetComponent<RectTransform>();
        hudRect.SetParent(hudsContainer.transform, false);

        hudRect.anchorMin = playerHUDPosPresets[slotIndex].anchorMin;
        hudRect.anchorMax = playerHUDPosPresets[slotIndex].anchorMax;
        hudRect.anchoredPosition = playerHUDPosPresets[slotIndex].position;

        SetOnAnyActionPerformed(player);
        GameController.Instance.ResetScore();


        //
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<Rigidbody2D>().simulated = false;
//        instanceScript.statsHUD.SetActive(false);
        colourpicker.GetComponent<Colourpicker>().Show(player,instanceScript.gameObject);
    }


    public void ShowAllHuds()
    {
        foreach (GameObject canva in playersCanvas)
        {
            if (canva == null) continue;
            LeanTween.cancel(canva);
            canva.SetActive(true);
            canva.GetComponent<CanvasGroup>().alpha = 1.0f;
        }
    }

    public void ShowAllHuds(float time)
    {
        foreach (GameObject canva in playersCanvas)
        {
            if (canva == null) continue;
            LeanTween.cancel(canva);
            LeanTween.value(canva, 0f, 1f, time).setOnStart(() =>
            {
                canva.SetActive(true);
                canva.GetComponent<CanvasGroup>().alpha = 0f;
            }).setOnUpdate((float r) => {
                canva.GetComponent<CanvasGroup>().alpha = r;
            });
        }
    }

    public void HideAllHuds()
    {
        foreach (GameObject canva in playersCanvas)
        {
            if (canva == null) continue;
            LeanTween.cancel(canva);
            canva.GetComponent<CanvasGroup>().alpha = 0f;
            canva.SetActive(false);
        }
    }

    public void HideAllHuds(float time)
    {
        foreach (GameObject canva in playersCanvas)
        {
            if (canva == null) continue;
            LeanTween.cancel(canva);
            LeanTween.value(canva, 1f, 0f, time).setOnStart(() =>
            {
                canva.SetActive(true);
                canva.GetComponent<CanvasGroup>().alpha = 1f;
            }).setOnUpdate((float r) => {
                canva.GetComponent<CanvasGroup>().alpha = r;
                canva.SetActive(false);
            });
        }
    }

    public void LockPlayersMovement()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            player.GetComponent<Player>().LockMovement();
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            AudioManager.instance.StopMusic();
        }
    }

    public void UnlockPlayersMovement()
    {
        foreach (GameObject player in players)
            player.GetComponent<Player>().UnlockMovement();
        //AudioManager.instance.SetMusicVolume(1f);

       // AudioManager.instance.SetMusicVolume(0.3f);
    }

    public void SetJoining(bool enabled) {
        if(enabled)
            playerInputManager.EnableJoining();
        else 
            playerInputManager.DisableJoining();
    }
    public void DisablePlayersCreation() {
        foreach (GameObject player in players)
            player.GetComponent<Items>().LockObjectCreation();
    }
    public void EnablePlayersCreation()
    {
        foreach (GameObject player in players)
            player.GetComponent<Items>().UnlockObjectCreation();
    }
    private void SetOnAnyActionPerformed(GameObject newPlayer)
    {
        newPlayer.GetComponent<PlayerInput>().actions.FindAction("Jump").started += CallOnAnyActionPerformed;
        newPlayer.GetComponent<PlayerInput>().actions.FindAction("GenerateSmallObject").started += CallOnAnyActionPerformed;
        newPlayer.GetComponent<PlayerInput>().actions.FindAction("GenerateMidObject").started += CallOnAnyActionPerformed;
        newPlayer.GetComponent<PlayerInput>().actions.FindAction("GenerateBigObject").started += CallOnAnyActionPerformed;
    }
    private void CallOnAnyActionPerformed(InputAction.CallbackContext context)
    {
        onAnyActionPerformed.Invoke();
    }
    public void RemovePlayersItems()
    {
        foreach (GameObject player in players)
            player.GetComponent<Items>().RemoveItem();
     }
    public void LockPlayersPhysics(bool value)
    {
        foreach (GameObject player in players)
            player.GetComponent<Rigidbody2D>().simulated = !value;
    }
}
