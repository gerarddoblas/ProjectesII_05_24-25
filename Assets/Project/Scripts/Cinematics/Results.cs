using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Results : MonoBehaviour
{
    // Start is called before the first frame update
    float timer = 5f;
    private void Start()
    {
        PlayersManager.Instance.StopPlayers();
        PlayersManager.Instance.LockPlayersMovement();
        PlayersManager.Instance.HideAllHuds();
        //GameObject [] orderedPlayers = PlayersManager.Instance.players.OrderBy(go => (-1*go.GetComponent<Player>().Score)).ToArray();
        GameObject[] orderedPlayers = PlayersManager.Instance.players.OrderByDescending(go => GameController.Instance.playerScores[PlayersManager.Instance.players.IndexOf(go)]).ToArray();
        for (int i = 0; i < orderedPlayers.Length; i++)
                orderedPlayers[i].transform.position = this.transform.GetChild(i).position;
    }

    void Update()
    {
        timer-=Time.deltaTime;
        if (Input.anyKeyDown&&timer<=0)
        {
            CameraFX.Instance.VerticalClap(() =>{
                PlayersManager.Instance.UnlockPlayersMovement();
                PlayersManager.Instance.playerInputManager.EnableJoining();
                SceneManager.LoadScene("PlayAgain");
            });
        }
    }
}
