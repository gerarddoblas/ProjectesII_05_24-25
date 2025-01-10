using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayersManager : MonoBehaviour
{
    public bool enabledHUDByDefault;
    public Color[] playerColours;
    public GameObject canvasPrefab;
    public List<GameObject> players;
    public List<GameObject> playersCanvas;
    public PlayerInputManager playerInputManager;
    public GameObject playerContainer, hudsContainer;
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

        SceneManager.sceneLoaded+=SetPlayersPosition;
    }
    private void SetPlayersPosition(Scene loadedScene, LoadSceneMode loadedSceneMode) {
        foreach (GameObject player in players)
            player.transform.position = Vector3.zero;
    }
    private void Start()
    {
        source = GetComponent<AudioSource>();
        playerInputManager = GetComponent<PlayerInputManager>();
        //playerInputManager.EnableJoining();
        playerInputManager.onPlayerJoined += OnPlayerJoin;
        //playerInputManager.onPlayerLeft += OnPlayerLeft;
    }
    private void OnPlayerJoin(PlayerInput input)
    {
        Debug.Log("hi");
        input.gameObject.transform.SetParent(playerContainer.transform);     
        input.gameObject.GetComponent<SpriteRenderer>().color = playerColours[players.Count];
        GameObject instantiatedHUD = GameObject.Instantiate(canvasPrefab,hudsContainer.transform);
        playersCanvas.Add(instantiatedHUD);
        instantiatedHUD.GetComponent<PlayerHud>().backgroundImage.GetComponent<Image>().color = playerColours[players.Count];
        players.Add(input.gameObject);

        instantiatedHUD.GetComponent<PlayerHud>().playerTransform = input.gameObject.transform;

        //if (input.currentControlScheme.Contains("Keyboard"))
        //    instantiatedHUD.GetComponent<PlayerHud>().SetKeyboardControls();
        //else
        //    instantiatedHUD.GetComponent<PlayerHud>().SetGamepdControls();
        //input.gameObject.GetComponent<HealthBehaviour>().OnAlterHealth.AddListener((int health, int maxHealth) => 
        //{
        //    instantiatedHUD.GetComponent<PlayerHud>().knockoutSlider.value = 1-((float)health/(float)maxHealth);
        //});
        //input.gameObject.GetComponent<Items>().onAlterMana.AddListener((float currentMana) =>
        //{
        //    instantiatedHUD.GetComponent<PlayerHud>().manaSlider.value = currentMana;
        //});
        //input.gameObject.GetComponent<Items>().onGenerateRandomSmallObject.AddListener(delegate(Sprite s)
        //{
        //    instantiatedHUD.GetComponent<PlayerHud>().smallImage.sprite = s;
        //});
        //input.gameObject.GetComponent<Items>().onGenerateRandomMidObject.AddListener(delegate (Sprite s)
        //{
        //    instantiatedHUD.GetComponent<PlayerHud>().midImage.sprite = s;
        //});
        //input.gameObject.GetComponent<Items>().onGenerateRandomBigObject.AddListener(delegate (Sprite s)
        //{
        //    instantiatedHUD.GetComponent<PlayerHud>().bigImage.sprite = s;
        //});
        //input.gameObject.GetComponent<Player>().onAlterScore.AddListener((float score) =>
        //{
        //    instantiatedHUD.GetComponent<PlayerHud>().scoreText.text = "Score: " + (int)score;
        //});

        input.gameObject.GetComponent<Items>().onAlterMana.AddListener((float currentMana) =>
        {
            instantiatedHUD.GetComponent<PlayerHud>().manaRadial.fillAmount = currentMana / 3;
        });
        input.gameObject.GetComponent<HealthBehaviour>().OnAlterHealth.AddListener((int health, int maxHealth) =>
        {
            instantiatedHUD.GetComponent<PlayerHud>().knockoutRadial.fillAmount = 1 - ((float)health / (float)maxHealth);
        });

        //float initialpos = instantiatedHUD.GetComponent<RectTransform>().sizeDelta.x;
        //foreach (Transform hud in hudsContainer.transform) {
        //    hud.position = new Vector3(initialpos,hud.GetComponent<RectTransform>().sizeDelta.y/1.5f, 0);
        //    initialpos += instantiatedHUD.GetComponent<RectTransform>().sizeDelta.x*2;
        //}
        if (!enabledHUDByDefault)
            instantiatedHUD.SetActive(false);
        //source.Play();
    }/*
    private void OnPlayerLeft(PlayerInput input)
    {
        for (int i = 0; i < playerInputManager.playerCount; i++) {
            if (input.gameObject == playerContainer.transform.GetChild(i)) {
                Destroy(playerContainer.transform.GetChild(i));
                Destroy(hudsContainer.transform.GetChild(i));
            } 
        }
    }*/
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
}
