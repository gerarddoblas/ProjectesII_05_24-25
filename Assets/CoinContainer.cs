using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinContainer : MonoBehaviour
{
    public static CoinContainer Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
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
