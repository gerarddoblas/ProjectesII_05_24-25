using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayersManager : MonoBehaviour
{
    public bool enabledHUDByDefault;
    public PlayerInputManager playerInputManager;
    [SerializeField] private GameObject playerContainer, hudsContainer;

    [Header("Lists")]
    public List<GameObject> players;
    public List<GameObject> playersCanvas;

    [Header("Visual Parameters")]
    [SerializeField] private Color[] playerColours;
    [SerializeField] private GameObject canvasPrefab;

    private AudioSource source;
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
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
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
            instanceScript.knockoutRadial.fillAmount = 1 - ((float)health / (float)maxHealth);
        });

        if (!enabledHUDByDefault)
            instantiatedHUD.SetActive(false);

        source.Play();
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
}
