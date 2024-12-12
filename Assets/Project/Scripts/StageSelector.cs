using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StageSelector : MonoBehaviour
{
    [SerializeField] string[] stages ;
    private int players;
    public void LoadRandomStage()
    {        
        SceneManager.LoadScene(stages[(int)Random.Range(0,stages.Length-1)]);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        players++;
        if (players == PlayersManager.Instance.players.Count)
        {
            LoadRandomStage();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        players--;
    }
}
