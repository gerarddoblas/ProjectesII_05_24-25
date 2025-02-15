using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public List<BaseGame> gameModes;
    public List<string> stages;
    public List<float> playerScores;
    public BaseGame currentGameMode;
    [SerializeField] bool clapAnimations;
    public int targetScore;
    public static GameController Instance { get; private set; }

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
    private void Start()
    {
        SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadedSceneMode)
        {
            if (clapAnimations)
            {
                try
                {
                    CameraFX.Instance.VerticalClap(delegate ()
                    {
                        currentGameMode.StartGame();
                    });
                }
                catch (Exception e) { }
            }
            else
                currentGameMode.StartGame();
        };
        
    }
    public void StartGames()
    {
        targetScore = 100;
        ResetScore();
        SelectNextGame();
        SelectNextLevel();
    }
    private void Update()
    {
        if(currentGameMode)
            currentGameMode.UpdateGame();
    }
    public void StartGames(int newTargetedScore)
    {
        targetScore = newTargetedScore;
        ResetScore();
        SelectNextGame();
        SelectNextLevel();
        currentGameMode.StartGame();
    }
    public void AddScore(float scoreToAdd, GameObject player)
    {
        for (int i = 0; i < PlayersManager.Instance.players.Count; i++)
            if(player == PlayersManager.Instance.players[i])
                playerScores[i]+=scoreToAdd;
    }
    public void ResetScore() {
        playerScores.Clear();
        for (int i = 0; i < PlayersManager.Instance.players.Count; i++)
            playerScores.Add(0);
    }
    private void SelectNextLevel() {
        SceneManager.LoadScene(stages[(int)UnityEngine.Random.Range(0, stages.Count - 1)]);
    }
    private void SelectNextGame()
    {
        currentGameMode = gameModes[(int)UnityEngine.Random.Range(0, gameModes.Count - 1)];
    }
    public void NextGame()
    {
        if (clapAnimations && CameraFX.Instance!=null)
        {
            CameraFX.Instance.VerticalClap(delegate () { 
                SelectNextGame();
                SelectNextLevel();
            });        
        }
        else { 
            SelectNextGame();
            SelectNextLevel();
        }
    }
}
