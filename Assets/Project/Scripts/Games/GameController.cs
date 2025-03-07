using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
                    if(currentGameMode.instructions != null)    
                        StartCoroutine(StartGameWithInstructions());
                    else
                        currentGameMode.StartGame();
                }
                catch (Exception e) { }
            }
            else
                currentGameMode.StartGame();
        };
        
    }
    IEnumerator StartGameWithInstructions()
    {
        yield return null;
        currentGameMode.SetGameState(false);
        PlayersManager.Instance.playerInputManager.DisableJoining();
        PlayersManager.Instance.LockPlayersMovement();
        CameraFX.Instance.SetClap();
        CameraFX.Instance.timer.gameObject.SetActive(false);
        PlayersManager.Instance.HideAllHuds();
        yield return null;
        CameraFX.Instance.instructions.color = new Color(255, 255, 255, 1);
        yield return null;
        CameraFX.Instance.instructions.sprite = currentGameMode.instructions;
        yield return new WaitForSeconds(2);
        CameraFX.Instance.instructions.color = new Color(255, 255, 255, 0);
        
        yield return null;
        CameraFX.Instance.ReverseVerticalClap(2,delegate ()
        {
            PlayersManager.Instance.ShowAllHuds(2);
            PlayersManager.Instance.UnlockPlayersMovement();
            CameraFX.Instance.timer.gameObject.SetActive(true);
            currentGameMode.StartGame();

        });
    }
    public void StartGames()
    {
        //targetScore = 100;
        PlayersManager.Instance.playerInputManager.DisableJoining();
        ResetScore();
        SelectNextGame();
        SelectNextLevel();
        currentGameMode.StartGame();
    }
    private void Update()
    {
        if(currentGameMode)
            currentGameMode.UpdateGame();
    }
    public void StartGames(int newTargetedScore)
    {
        targetScore = newTargetedScore;
        StartGames();
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
    public void RemoveScore(float scoreToRemove, GameObject player)
    {
        for (int i = 0; i < PlayersManager.Instance.players.Count; i++)
        {
            if (player == PlayersManager.Instance.players[i])
            {
                playerScores[i] -= scoreToRemove;
                if (playerScores[i] < 0)
                    playerScores[i] = 0;
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
