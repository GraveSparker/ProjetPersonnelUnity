using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody;

    private float halfHeight;

    void Start()
    {
        halfHeight = spriteRenderer.bounds.extents.y;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            other.GetComponent<Coin>()?.CollectCoins();
        }
        else if (other.CompareTag("HealthPickup"))
        {
            other.GetComponent<HealthPickup>()?.CollectHealth();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            CollideWithEnemy(other);
        }
    }

    private void CollideWithEnemy(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        enemy.HitPlayer(transform);
    }
}
