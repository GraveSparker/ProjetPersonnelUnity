using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup winScreenCanvasGroup;
    [SerializeField] private float winFadeDuration = 0.75f;

    public string transitionedFromScene;

    public Vector2 platformingRespawnPoint;

    public bool canRespawnPlayer = true;

    public GameObject gameOverUI;
    public GameObject mainMenu;
    public GameObject winScreen;
    public GameObject healthBar;
    public GameObject restartButton;
    public GameObject restartBossButton;

    public HashSet<string> CollectedCoins = new HashSet<string>();
    public HashSet<string> CollectedHealth = new HashSet<string>();


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
        if (gameOverUI.activeInHierarchy || mainMenu.activeInHierarchy || winScreen.activeInHierarchy)
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

    private IEnumerator FadeInWinScreen()
    {
        float elapsed = 0f;

        yield return new WaitForSecondsRealtime(1f);

        while (elapsed < winFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            winScreenCanvasGroup.alpha = Mathf.Clamp01(elapsed / winFadeDuration);
            yield return null;
        }

        winScreenCanvasGroup.alpha = 1f;
    }

    private IEnumerator StopTime()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0f;
    }

    public void ResetCollectedHealthOnDeath()
    {
        CollectedHealth.Clear();
    }

    public void gameOver()
    {
        canRespawnPlayer = true;
        gameOverUI.SetActive(true);
        restartButton.SetActive(true);
        restartBossButton.SetActive(false);
        Time.timeScale = 0f;   // freeze game
    }

    public void gameOverInBoss()
    {
        canRespawnPlayer = false;
        gameOverUI.SetActive(true);
        restartBossButton.SetActive(true);
        restartButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        winScreen.SetActive(true);

        // Disable player
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.enabled = false;
        }

        if (FindAnyObjectByType<PlayerAttack>() != null)
        {
            FindAnyObjectByType<PlayerAttack>().enabled = false;
        }
        healthBar.SetActive(false);

        // Prepare fade
        winScreenCanvasGroup.alpha = 0f;

        // Start fade using unscaled time
        StartCoroutine(FadeInWinScreen());

        StartCoroutine(StopTime());
    }

    public void restart()
    {
        Time.timeScale = 1f;   // unfreeze
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameOverUI.SetActive(false);
    }

    public void restartBoss()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Room_2");
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
