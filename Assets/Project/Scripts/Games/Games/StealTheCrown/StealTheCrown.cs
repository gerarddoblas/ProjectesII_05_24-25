using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StealTheCrown", menuName = "Games/TimeBasedGame/StealTheCrown")]
public class StealTheCrown : TimeBasedGame
{
    [SerializeField] GameObject crownPrefab;
    public GameObject instantiatedCrown;
    private GameObject CrownOwner;
    public bool scoreOverTime = false;
    public int scoreToAdd = 50;
    override public void StartGame()
    {
        base.StartGame();
    }

    override public void UpdateGame()
    {
        base.UpdateGame();
        try {
            //Debug.Log("Trying to add score to:" + Crown.Instance.GetOwner().gameObject);
            if (scoreOverTime && Crown.Instance.GetOwner().gameObject != null)
            {
                Debug.Log("AddingScore");
                GameController.Instance.AddScore(scoreToAdd * Time.deltaTime, Crown.Instance.GetOwner().gameObject);
            }
         }catch(Exception e) { Debug.LogWarning(e); }
    }
    public override void FinishGame()
    {
        //Debug.Log("Trying to add score to:" + Crown.Instance.GetOwner().gameObject);
        if (!scoreOverTime && Crown.Instance.GetOwner().gameObject != null)
        {
            Debug.Log("AddingScore");
            GameController.Instance.AddScore(scoreToAdd, Crown.Instance.GetOwner().gameObject);
        }
        base.FinishGame();
    }
}
