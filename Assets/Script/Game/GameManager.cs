using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string transitionedFromScene;

    public Vector2 platformingRespawnPoint;

    public GameObject gameOverUI;
    public GameObject mainMenu;

    public HashSet<string> CollectedCoins = new HashSet<string>();


    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (gameOverUI.activeInHierarchy || mainMenu.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find all SceneTransition objects in the newly loaded scene
        var transitions = FindObjectsByType<SceneTransition>(FindObjectsSortMode.None);
        foreach (var t in transitions)
        {
            // If the transition's "TransitionTo" equals the scene we came from, place player at this transition's start point
            if (t.TransitionTo == transitionedFromScene)
            {
                if (PlayerMovement.Instance != null)
                {
                    PlayerMovement.Instance.transform.position = t.StartPoint.position;
                    PlayerMovement.Instance.StartCoroutine(
                        PlayerMovement.Instance.WalkIntoNewScene(t.ExitDirection, t.ExitTime)
                    );
                }
                StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));
                break;
            }
        }
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;   // freeze game
    }

    public void restart()
    {
        Time.timeScale = 1f;   // unfreeze
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameOverUI.SetActive(false);
    }

    public void play()
    {
        SceneManager.LoadScene("Room_Start");
        mainMenu.SetActive(false);
    }

    public void quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
