using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public List<BaseGame> gameModes;
    public List<string> stages;
    public List<float> playerScores;
    public List<uint> playerGameScores;
    public BaseGame currentGameMode;
    [SerializeField] bool clapAnimations;
    public int targetScore;

    [SerializeField] GameObject physicsCoin;
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
                if (currentGameMode != null)
                {
                    if (currentGameMode.instructions != null)
                        StartCoroutine(StartGameWithInstructions());
                    else
                        currentGameMode.StartGame();
                }
            }
            else
                currentGameMode.StartGame();
        };
        
    }
    IEnumerator StartGameWithInstructions()
    {
        currentGameMode.SetGameState(false);
        PlayersManager.Instance.playerInputManager.DisableJoining();
        PlayersManager.Instance.LockPlayersMovement();
        CameraFX.Instance.SetClap();
        CameraFX.Instance.timer.gameObject.SetActive(false);
        PlayersManager.Instance.HideAllHuds();
        CameraFX.Instance.instructions.color = new Color(255, 255, 255, 1);
        CameraFX.Instance.instructions.sprite = currentGameMode.instructions;
        yield return new WaitForSeconds(5);
        CameraFX.Instance.instructions.color = new Color(255, 255, 255, 0);
        
        yield return null;
        CameraFX.Instance.ReverseVerticalClap(2,delegate ()
        {
            PlayersManager.Instance.ShowAllHuds(2);
            PlayersManager.Instance.UnlockPlayersMovement();
            CameraFX.Instance.timer.gameObject.SetActive(true);
            currentGameMode.StartGame();
            AudioManager.instance.PlaySFX("NowGo");
            AudioManager.instance.PlayMusic("BackGround");
        });
    }
    
    public void StartGames()
    {
        //targetScore = 100;
        PlayersManager.Instance.playerInputManager.DisableJoining();
        ResetScore();
        ResetGameScore();
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
                if(currentGameMode.GetType().Equals(typeof(CoinCollectGame)))
                {
                    for(int j = 0; j < scoreToRemove; j++)
                    {
                        GameObject instance = Instantiate(physicsCoin, player.transform.position + new Vector3(0, 2), Quaternion.identity);
                        instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-10, 10), 5));
                    }
                }
                else if (GameController.Instance.currentGameMode.GetType() == typeof(StealTheCrown))
                {
                    try
                    {
                        if(Crown.Instance.GetOwner().gameObject == player)
                        {
                            Crown.Instance.RemoveOwner();
                        }
                    }catch (Exception e) { }
                }
                if (playerScores[i] < 0 && !currentGameMode.GetType().Equals(typeof(FightArenaGame)))
                    playerScores[i] = 0;
                PlayersManager.Instance.playersCanvas[i].GetComponent<PlayerHud>().SetScoreText((int)playerScores[i]);
            }
        }
    }
    public void ResetScore() {
        playerScores.Clear();
        for (int i = 0; i < PlayersManager.Instance.players.Count; i++)
        {
            playerScores.Add(0);
            PlayersManager.Instance.playersCanvas[i].GetComponent<PlayerHud>().SetScoreText((int)playerScores[i]);
        }
            
    }

    public void ResetGameScore()
    {
        playerGameScores.Clear();
        for (int i = 0; i < PlayersManager.Instance.players.Count; i++)
        {
            playerGameScores.Add(0);
        }

    }

    private void UpdateGameScores()
    {
        List<int> maxScoreIndexes = new List<int>();
        maxScoreIndexes.Add(0);
        for (int i = 1; i < playerScores.Count; i++)
            if (playerGameScores[i] > playerGameScores[maxScoreIndexes[0]])
                maxScoreIndexes = new List<int> { i };
            else if (playerGameScores[i] == playerGameScores[maxScoreIndexes[0]])
                maxScoreIndexes.Add(i);
        foreach(int i in maxScoreIndexes)
            playerGameScores[i]++;
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
        PlayersManager.Instance.LockPlayersMovement();
        PlayersManager.Instance.StopPlayers();
        PlayersManager.Instance.HealAllPlayers();
        UpdateGameScores();
        ResetScore();
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
            currentGameMode = null;

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
        for(int i = 0; i < playerGameScores.Count;i++)
            if (playerGameScores[i]>=targetScore)
                return true;
        return false;
    }
}
