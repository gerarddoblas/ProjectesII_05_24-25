using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinContainer : MonoBehaviour
{
    private void Awake()
    {
        if (GameController.Instance.currentGameMode.GetType().Equals(typeof(CoinCollectGame)))
            EnableCoins();
        else
            DisableCoins();
    }
    public void EnableCoins()
    {
        for(int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
    }
    public void DisableCoins()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
}
