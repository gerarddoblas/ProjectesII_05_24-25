using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Results : MonoBehaviour
{
    // Start is called before the first frame update
    float timer = 5f;
    private void Awake()
    {
        PlayersManager.Instance.StopPlayers();
        PlayersManager.Instance.LockPlayersMovement();
        PlayersManager.Instance.HideAllHuds();
        //GameObject [] orderedPlayers = PlayersManager.Instance.players.OrderBy(go => (-1*go.GetComponent<Player>().Score)).ToArray();
        GameObject[] orderedPlayers = PlayersManager.Instance.players.OrderByDescending(go => GameController.Instance.playerScores[PlayersManager.Instance.players.IndexOf(go)]).ToArray();
        for (int i = 0; i < orderedPlayers.Length; i++)
        {
            Debug.Log("Player with colour " + orderedPlayers[i].GetComponent<SpriteRenderer>().color 
                +" lasted in " + i + " place with  "+ orderedPlayers[i].GetComponent<Player>().Score +" points");
            if (i < 3)
            {
                orderedPlayers[i].transform.position = this.transform.GetChild(i).position;
            }
            else
            {
                orderedPlayers[i].transform.position = this.transform.GetChild(3).position;
            }
        }
        
    }

    void Update()
    {
        timer-=Time.deltaTime;
        if (Input.anyKeyDown&&timer<=0)
        {
            CameraFX.Instance.VerticalClap(() =>{
                PlayersManager.Instance.UnlockPlayersMovement();
                PlayersManager.Instance.playerInputManager.EnableJoining();
                SceneManager.LoadScene("LevelSelector");
            });
        }
    }
}
