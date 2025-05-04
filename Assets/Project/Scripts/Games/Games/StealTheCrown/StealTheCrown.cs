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
    [SerializeField] private bool scoreOverTime = false;
    [SerializeField] private int scoreToAdd = 50;
    override public void StartGame()
    {
        base.StartGame();
    }

    override public void UpdateGame()
    {
        base.UpdateGame();
        if (scoreOverTime && Crown.Instance.GetOwner() != null)
            GameController.Instance.AddScore(scoreToAdd * Time.deltaTime, Crown.Instance.GetOwner().gameObject);
    }
    public override void FinishGame()
    {
        if (!scoreOverTime && Crown.Instance.GetOwner().gameObject != null)
            GameController.Instance.AddScore(scoreToAdd, Crown.Instance.GetOwner().gameObject);
        base.FinishGame();
    }
}
