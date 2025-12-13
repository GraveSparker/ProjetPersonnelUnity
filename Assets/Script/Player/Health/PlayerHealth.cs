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
    public static Action OnPlayerDie;

    private bool isDead;


    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        currentHealth = health;
        maxHealth = health;
    }

    public void DamagePlayer(int damageAmount)
    {
        currentHealth -= damageAmount;
        OnPlayerTakeDamage?.Invoke(currentHealth);

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            OnPlayerDie?.Invoke();
            playerMovementState.SetMoveState(PlayerMovementState.MoveState.Death);
            StartCoroutine(DeathRoutine());
        }
        else if (currentHealth > 0)
        {
            StartCoroutine(Invulnerability());
            playerMovementState.SetMoveState(PlayerMovementState.MoveState.Hurt);
            StartCoroutine(HurtStateLock());
        }
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
    }

    private IEnumerator HurtStateLock()
    {
        // Wait a bit so the animation plays
        yield return new WaitForSeconds(0.2f);
        playerMovementState.SetMoveState(PlayerMovementState.MoveState.Idle);
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(1f); // animation length
        gameManager.gameOver();
        Destroy(gameObject);
    }
}
