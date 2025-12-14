using UnityEngine;

public class ShockwaveHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 2;
    [SerializeField] private Vector2 knockback = new Vector2(6f, 4f);
    [SerializeField] private float lifeTime = 0.2f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        int direction = other.transform.position.x > transform.position.x ? 1 : -1;

        FindAnyObjectByType<PlayerHealth>()
            .DamagePlayer(damage);

        FindAnyObjectByType<PlayerMovement>()
            .KnockbackPlayer(knockback, direction);
    }
}
