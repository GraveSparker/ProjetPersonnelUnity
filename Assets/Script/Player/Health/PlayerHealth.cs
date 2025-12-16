using UnityEngine;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int health = 10;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PlayerMovementState playerMovementState;

    public int currentHealth { get; private set; }
    public int maxHealth { get; private set; }

    public GameManager gameManager;
    public static Action<int> OnPlayerTakeDamage;
    public static Action<int> OnPlayerRestoreHealth;
    public static Action OnPlayerDie;

    private bool isDead;
    private bool isInvulnerable;


    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        currentHealth = health;
        maxHealth = health;
    }

    public void DamagePlayer(int damageAmount)
    {
        if (isInvulnerable || isDead)
            return;

        isInvulnerable = true; //LOCK IMMEDIATELY

        currentHealth -= damageAmount;
        OnPlayerTakeDamage?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            OnPlayerDie?.Invoke();
            playerMovementState.SetMoveState(PlayerMovementState.MoveState.Death);
            StartCoroutine(DeathRoutine());
        }
        else
        {
            StartCoroutine(Invulnerability());
            playerMovementState.SetMoveState(PlayerMovementState.MoveState.Hurt);
            StartCoroutine(HurtStateLock());
        }
    }

    private void RestoreHealth(int healthRestored)
    {
        currentHealth += healthRestored;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnPlayerRestoreHealth?.Invoke(currentHealth);
    }

    private void OnEnable()
    {
        HealthPickup.OnHealthCollected += RestoreHealth;
    }

    private void OnDisable()
    {
        HealthPickup.OnHealthCollected -= RestoreHealth;
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(6, 8, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes));
        }
        Physics2D.IgnoreLayerCollision(6, 8, false);

        isInvulnerable = false;
    }

    private IEnumerator HurtStateLock()
    {
        // Wait a bit so the animation plays
        yield return new WaitForSeconds(0.2f);
        playerMovementState.SetMoveState(PlayerMovementState.MoveState.Idle);
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(0f); // animation length
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (currentScene == "Room_Boss")
        {
            //NO RESPAWN IN BOSS ROOM
            gameManager.ResetCollectedHealthOnDeath();
            gameManager.gameOverInBoss();
            Destroy(gameObject);
        }
        else
        {
            //NORMAL DEATH FLOW (respawn handled elsewhere)
            gameManager.ResetCollectedHealthOnDeath();
            gameManager.gameOver();
            Destroy(gameObject);
        }
    }
}
