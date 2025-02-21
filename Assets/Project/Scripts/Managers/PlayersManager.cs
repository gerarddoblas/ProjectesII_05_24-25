using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayersManager : MonoBehaviour
{
    [SerializeField] bool enabledHUDByDefault, enableCreationByDefault;
    public PlayerInputManager playerInputManager;
    [SerializeField] private GameObject playerContainer, hudsContainer;

    [Header("Lists")]
    public List<GameObject> players;
    public List<GameObject> playersCanvas;

    [Header("Visual Parameters")]
    [SerializeField] private Color[] playerColours;
    [SerializeField] private GameObject canvasPrefab;

    public UnityEvent onAnyActionPerformed;
    public static PlayersManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        SceneManager.sceneLoaded += SetPlayersPosition;
    }

    private void SetPlayersPosition(Scene loadedScene, LoadSceneMode loadedSceneMode) {
        foreach (GameObject player in players)
            player.transform.position = Vector3.zero;
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
        player.GetComponent<SpriteRenderer>().color = playerColours[players.Count];
        GameObject instantiatedHUD = GameObject.Instantiate(canvasPrefab,hudsContainer.transform);
        PlayerHud instanceScript = instantiatedHUD.GetComponent<PlayerHud>();

        playersCanvas.Add(instantiatedHUD);
        players.Add(input.gameObject);

        instanceScript.playerTransform = input.gameObject.transform;

        player.GetComponent<Items>().onAlterMana.AddListener((float currentMana) =>
        {
            instanceScript.manaRadial.fillAmount = currentMana / 3;
        });

        player.GetComponent<HealthBehaviour>().OnAlterHealth.AddListener((int health, int maxHealth) =>
        {
            instanceScript.barraDeVida.fillAmount = 1 - ((float)health / (float)maxHealth);
        });
        Vector3[] hudPositions = new Vector3[4];

        hudPositions[0] = new Vector3(-45, 24, 0);   
        hudPositions[1] = new Vector3(35, 24, 0);    
        hudPositions[2] = new Vector3(-45, -25, 0);  
        hudPositions[3] = new Vector3(20, -25, 0);   

        if (playersCanvas.Count <= 4)
        {
            instantiatedHUD.transform.position = hudPositions[playersCanvas.Count - 1]; // Asigna la posición según el índice del jugador
        }
        if (!enabledHUDByDefault)
            instantiatedHUD.SetActive(false);
        else
            instantiatedHUD.SetActive(true);

        if(!enableCreationByDefault)
            player.GetComponent<Items>().LockManaAndCreation();
        else
            player.GetComponent<Items>().UnlockManaAndCreation();

        SetOnAnyActionPerformed(player);
    }

    public void ShowAllHuds()
    {
        foreach (GameObject canva in playersCanvas)
        {
            LeanTween.cancel(canva);
            canva.SetActive(true);
            canva.GetComponent<CanvasGroup>().alpha = 1.0f;
        }
    }

    public void ShowAllHuds(float time)
    {
        foreach (GameObject canva in playersCanvas)
        {
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
            LeanTween.cancel(canva);
            canva.GetComponent<CanvasGroup>().alpha = 0f;
            canva.SetActive(false);
        }
    }

    public void HideAllHuds(float time)
    {
        foreach (GameObject canva in playersCanvas)
        {
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
        }
    }

    public void UnlockPlayersMovement()
    {
        foreach (GameObject player in players)
            player.GetComponent<Player>().UnlockMovement();
    }

    public void SetJoining(bool enabled) {
        if(enabled)
            playerInputManager.EnableJoining();
        else 
            playerInputManager.DisableJoining();
    }
    public void DisablePlayersCreation() {
        foreach (GameObject player in players)
            player.GetComponent<Items>().LockManaAndCreation();
    }
    public void EnablePlayersCreation()
    {
        foreach (GameObject player in players)
            player.GetComponent<Items>().UnlockManaAndCreation();
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
}
