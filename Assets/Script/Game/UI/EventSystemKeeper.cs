using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemKeeper : MonoBehaviour
{
    void OnEnable()
    {
        var systems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

        // If another EventSystem exists (not this one), destroy immediately
        foreach (var sys in systems)
        {
            if (sys != GetComponent<EventSystem>())
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
