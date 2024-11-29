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
    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.EnableJoining();
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

        if (input.currentControlScheme.Contains("Keyboard"))
            instantiatedHUD.GetComponent<PlayerHud>().SetKeyboardControls();
        else
            instantiatedHUD.GetComponent<PlayerHud>().SetGamepdControls();
        input.gameObject.GetComponent<HealthBehaviour>().OnAlterHealth.AddListener((int health, int maxHealth) => 
        {
            instantiatedHUD.GetComponent<PlayerHud>().knockoutSlider.value = 1-((float)health/(float)maxHealth);
        });
        input.gameObject.GetComponent<Items>().onAlterMana.AddListener((float currentMana) =>
        {
            instantiatedHUD.GetComponent<PlayerHud>().manaSlider.value = currentMana;
        });
        input.gameObject.GetComponent<Items>().onGenerateRandomSmallObject.AddListener(delegate(Sprite s)
        {
            instantiatedHUD.GetComponent<PlayerHud>().smallImage.sprite = s;
        });
        input.gameObject.GetComponent<Items>().onGenerateRandomMidObject.AddListener(delegate (Sprite s)
        {
            instantiatedHUD.GetComponent<PlayerHud>().midImage.sprite = s;
        });
        input.gameObject.GetComponent<Items>().onGenerateRandomBigObject.AddListener(delegate (Sprite s)
        {
            instantiatedHUD.GetComponent<PlayerHud>().bigImage.sprite = s;
        });
        input.gameObject.GetComponent<Player>().onAlterScore.AddListener((float score) =>
        {
            instantiatedHUD.GetComponent<PlayerHud>().scoreText.text = "Score: " + (int)score;
        });

        float initialpos = instantiatedHUD.GetComponent<RectTransform>().sizeDelta.x;
        foreach (Transform hud in hudsContainer.transform) {
            hud.position = new Vector3(initialpos,hud.GetComponent<RectTransform>().sizeDelta.y/1.5f, 0);
            initialpos += instantiatedHUD.GetComponent<RectTransform>().sizeDelta.x*2;
        }
        source.Play();
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
}
