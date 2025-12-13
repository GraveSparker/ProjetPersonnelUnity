using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private GameObject player;

    public static Action<GameObject> OnPlayerSpawned;

    private void Awake()
    {
        // If a PlayerMovement already exists (persistent), use it.
        if (PlayerMovement.Instance == null)
        {
            player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            DontDestroyOnLoad(player);
        }
        else
        {
            player = PlayerMovement.Instance.gameObject;
            // Optionally reposition the existing player to this scene's spawn point
        }
    }

    void Start()
    {
        OnPlayerSpawned?.Invoke(player);
    }

    //private void ResetScene()
    //{
    //    Invoke("ResetSceneDelay", 2f);
    //}

    //private void ResetSceneDelay()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}

    //private void OnEnable()
    //{
    //    PlayerHealth.OnPlayerDie += ResetScene;
    //}

    //private void OnDisable()
    //{
    //    PlayerHealth.OnPlayerDie -= ResetScene;
    //}
}
