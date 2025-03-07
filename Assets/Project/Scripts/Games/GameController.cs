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
        //targetScore = 100;
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
        for (int i = 0; i < PlayersManager.Instance.players.Count; i++){
            if (player == PlayersManager.Instance.players[i]){
                playerScores[i] += scoreToAdd;
                PlayersManager.Instance.playersCanvas[i].GetComponent<PlayerHud>().SetScoreText((int)playerScores[i]);
            }
        }
    }
    public void ResetScore() {
        playerScores.Clear();
        for (int i = 0; i < PlayersManager.Instance.players.Count; i++)
            playerScores.Add(0);
    }
    private void SelectNextLevel() {
        SceneManager.LoadScene(stages[(int)UnityEngine.Random.Range(0, stages.Count)]);
    }
    private void SelectNextGame()
    {
        currentGameMode = gameModes[(int)UnityEngine.Random.Range(0, gameModes.Count)];
    }
    public void NextGame()
    {
        if (!PlayerAchievedTargetScore())
        {
            if (clapAnimations && CameraFX.Instance != null)
            {
                CameraFX.Instance.VerticalClap(delegate ()
                {
                    SelectNextGame();
                    SelectNextLevel();
                });
            }
            else
            {
                SelectNextGame();
                SelectNextLevel();
            }
        }
        else
        {
            PlayersManager.Instance.HideAllHuds();
           
            if (clapAnimations && CameraFX.Instance != null)
            {
                CameraFX.Instance.VerticalClap(delegate ()
                {
                    SceneManager.LoadScene("ResultScene");
                });
            }
            else
                SceneManager.LoadScene("ResultScene");
        }

    }
    public bool PlayerAchievedTargetScore()
    {
        for(int i = 0; i < playerScores.Count;i++)
            if (playerScores[i]>=targetScore)
                return true;
        
        return false;
    }
}
