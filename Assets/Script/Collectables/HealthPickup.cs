using UnityEngine;
using System;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private string healthPickupID;
    [SerializeField] private int healthRecovered = 2;

    public static Action<int> OnHealthCollected;

    private void Start()
    {
        // If we already collected this health, delete it when scene loads
        if (GameManager.Instance.CollectedHealth.Contains(healthPickupID))
        {
            Destroy(gameObject);
        }
    }

    public void CollectHealth()
    {
        // Save as collected
        if (!GameManager.Instance.CollectedHealth.Contains(healthPickupID))
            GameManager.Instance.CollectedHealth.Add(healthPickupID);

        OnHealthCollected?.Invoke(healthRecovered);
        Destroy(gameObject);
    }
}
