//using System;
//using UnityEngine;
//using UnityEngine.UI;

//public class HUD : MonoBehaviour
//{
//    [SerializeField] private PlayerHealth playerHealth;
//    [SerializeField] private Image totalhealthbar;
//    [SerializeField] private Image currenthealthbar;

//    private void SetupHealthbar(GameObject player)
//    {
//        totalhealthbar.fillAmount = playerHealth.currentHealth / 10;
//    }

//    private void UpdateHealthbar(int currentHealth)
//    {
//        currenthealthbar.fillAmount = playerHealth.currentHealth / 10;
//    }

//    private void OnEnable()
//    {
//        GameController.OnPlayerSpawned += SetupHealthbar;
//        PlayerHealth.OnPlayerTakeDamage += UpdateHealthbar;
//    }


//    private void OnDisable()
//    {
//        GameController.OnPlayerSpawned -= SetupHealthbar;
//        PlayerHealth.OnPlayerTakeDamage -= UpdateHealthbar;
//    }
//}
