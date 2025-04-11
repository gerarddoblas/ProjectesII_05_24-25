using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TimeBasedGame", menuName = "Games/TimeBasedGame/CoinCollect")]
public class CoinCollectGame : TimeBasedGame
{

    public PrefabTile coinTile;

    override public void Reset()
    {
        base.Reset();
        coinTile.positions = new List<Vector3Int>();
        Debug.Log("Reset");
    }
    override public void StartGame()
    {
        base.StartGame();
        PlayersManager.Instance.SetJoining(false);
        remainingTime = gameTime;
        PlayersManager.Instance.ShowAllHuds(1);
        PlayersManager.Instance.EnablePlayersCreation();

        /////////////////////////////Game-specific
        TilemapScript.Instance.GetComponent<Tilemap>().RefreshAllTiles();
        Debug.Log(coinTile.positions.Count);
        coinTile.Spawn();
    }

    override public void UpdateGame()
    {
        base.UpdateGame();

    }
}

