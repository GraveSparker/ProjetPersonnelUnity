using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private HealthBarUI healthBarUI;
    [SerializeField] private TextMeshProUGUI counterText;


    private PlayerHealth currentPlayerHealth;
    private int coinCollectedAmount;

    private void OnEnable()
    {
        GameController.OnPlayerSpawned += OnPlayerSpawned;
        PlayerHealth.OnPlayerTakeDamage += OnPlayerTakeDamage; // static event on PlayerHealth
        Coin.OnCoinCollected += CollectCoin;
        PlayerHealth.OnPlayerDie += OnPlayerDie;               // optional: react to death
    }


    private void OnDisable()
    {
        GameController.OnPlayerSpawned -= OnPlayerSpawned;
        PlayerHealth.OnPlayerTakeDamage -= OnPlayerTakeDamage;
        Coin.OnCoinCollected -= CollectCoin;
        PlayerHealth.OnPlayerDie -= OnPlayerDie;
    }

    private void CollectCoin(int healthRestored)
    {
        coinCollectedAmount ++;
        counterText.text = $"x{coinCollectedAmount}";
    }

    private void OnPlayerSpawned(GameObject player)
    {
        currentPlayerHealth = player.GetComponent<PlayerHealth>();
        if (currentPlayerHealth == null)
        {
            Debug.LogWarning("HUDController: spawned player has no PlayerHealth component.");
            return;
        }

        // initialize the bar with current/max values
        healthBarUI.Initialize(currentPlayerHealth.currentHealth, currentPlayerHealth.maxHealth);
    }

    private void OnPlayerTakeDamage(int currentHealth)
    {
        // Guard: if we have a stored player, use its maxHealth; otherwise safe fallback
        int max = (currentPlayerHealth != null) ? currentPlayerHealth.maxHealth : 1;
        float normalized = (float)currentHealth / Mathf.Max(1, max);
        healthBarUI.SetFill(normalized);
    }

    private void OnPlayerDie()
    {
        // Immediately set to 0
        healthBarUI.SetFillInstant(0f);
    }
}
