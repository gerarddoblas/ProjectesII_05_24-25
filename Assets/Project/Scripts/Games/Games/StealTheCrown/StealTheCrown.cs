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
        instantiatedCrown = GameObject.Instantiate(crownPrefab);
    }

    override public void UpdateGame()
    {
        base.UpdateGame();
        try {
            if (scoreOverTime && instantiatedCrown.GetComponent<Crown>().GetOwner().gameObject != null)
                GameController.Instance.AddScore(scoreToAdd * Time.deltaTime, instantiatedCrown.GetComponent<Crown>().GetOwner().gameObject);
         }catch(Exception e) { }
    }
    public override void FinishGame()
    {
        if (!scoreOverTime&& instantiatedCrown.GetComponent<Crown>().GetOwner().gameObject != null)
            GameController.Instance.AddScore(scoreToAdd, instantiatedCrown.GetComponent<Crown>().GetOwner().gameObject);
        base.FinishGame();
    }
}
