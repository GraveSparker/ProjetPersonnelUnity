using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private EnemyMovement enemyMoveScript;
    [SerializeField] private Vector2 knockbackToSelf = new Vector2(3f, 5f);
    [SerializeField] private Vector2 knockbackToPlayer = new Vector2(3f, 5f);
    [SerializeField] private float knockbackDelayToSelf = 1.5f;
    [SerializeField] private int damage = 3;
    [SerializeField] private int health = 3;
    [SerializeField] private float recoilLength;
    [SerializeField] private float recoilFactor;
    [SerializeField] private bool isRecoiling = false;

    private float recoilTimer;

    private void Update()
    {
        if (health <= 0)
        {
            Die();
            return;
        }

        if (!isRecoiling)
            return;

        recoilTimer += Time.deltaTime;

        if (recoilTimer >= recoilLength)
        {
            // END RECOIL
            isRecoiling = false;
            enemyMoveScript.enabled = true; // movement returns
            recoilTimer = 0;
        }
    }


    public void EnemyHit(int _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;

        // Start recoil
        isRecoiling = true;
        recoilTimer = 0f;

        // Stop enemy movement instantly
        rigidBody.linearVelocity = Vector2.zero;
        enemyMoveScript.enabled = false;

        // Apply proper knockback
        Vector2 force = _hitDirection.normalized * _hitForce * recoilFactor;
        rigidBody.AddForce(force, ForceMode2D.Impulse);
    }


    public void Die()
    {
        Destroy(gameObject);
    }

    public void HitPlayer(Transform playerTransform)
    {
        int direction = GetDirection(playerTransform);
        FindAnyObjectByType<PlayerMovement>().KnockbackPlayer(knockbackToPlayer, direction);
        FindAnyObjectByType<PlayerHealth>().DamagePlayer(damage);
        GetComponent<EnemyMovement>().KnockbackEnemy(knockbackToSelf, -direction, knockbackDelayToSelf);
    }

    private int GetDirection(Transform playerTransform)
    {
        if (transform.position.x > playerTransform.position.x)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

}
