using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "StealTheCrown", menuName = "Games/TimeBasedGame/StealTheCrown")]
public class StealTheCrown : TimeBasedGame
{
    [SerializeField] GameObject crownPrefab;
    public GameObject instantiatedCrown;
    private GameObject CrownOwner;
    override public void StartGame()
    {
        base.StartGame();
        GameObject.Instantiate(crownPrefab);
    }

    override public void UpdateGame()
    {
        base.UpdateGame();

    }
    public override void FinishGame()
    {
        GameController.Instance.AddScore(50,instantiatedCrown.GetComponent<Crown>().GetOwner());
        base.FinishGame();
    }
}
