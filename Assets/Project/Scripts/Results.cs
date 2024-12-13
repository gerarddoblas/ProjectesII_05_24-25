using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Results : MonoBehaviour
{
    // Start is called before the first frame update
    float timer = 5f;
    void Start()
    {
        PlayersManager.Instance.HideAllHuds();
        PlayersManager.Instance.LockPlayersMovement();
        GameObject [] orderedPlayers = PlayersManager.Instance.players.OrderBy(go => go.GetComponent<Player>().Score).ToArray();
        for(int i = 0; i < orderedPlayers.Length; i++)
        {
            if (i < 3)
                orderedPlayers[i].transform.position = this.transform.GetChild(i).position;
            else
                orderedPlayers[i].transform.position = this.transform.GetChild(3).position;
        }
    }

    void Update()
    {
        timer-=Time.deltaTime;
        if (Input.anyKeyDown&&timer<=0)
        {
            PlayersManager.Instance.UnlockPlayersMovement();
            SceneManager.LoadScene("GrayBox");
        }
    }
}
