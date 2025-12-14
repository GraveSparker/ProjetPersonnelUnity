using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private string coinID;  // Unique ID for this coin
    [SerializeField] private int coinRecovered = 0; //Does nothing (not ChatGPT)

    public static Action<int> OnCoinCollected;

    private void Start()
    {
        // If we already collected this coin, delete it when scene loads
        if (GameManager.Instance.CollectedCoins.Contains(coinID))
        {
            Destroy(gameObject);
        }
    }

    public void CollectCoins()
    {
        // Save as collected
        if (!GameManager.Instance.CollectedCoins.Contains(coinID))
            GameManager.Instance.CollectedCoins.Add(coinID);

        OnCoinCollected?.Invoke(coinRecovered);
        Destroy(gameObject);
    }
}
