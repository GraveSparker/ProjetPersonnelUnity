using UnityEngine;

public class BossJump : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject shockwavePrefab;
    [SerializeField] private Transform shockwaveSpawnPoint;

    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float delayBetweenJumps = 2f;

    private float jumpTimer;
    private bool isGrounded;
    private bool wasGrounded;
    private bool hasJumped;

    private void Update()
    {
        if (!enabled) return;

        // recoil safety
        if (enemy != null && enemy.enabled == false)
            return;

        jumpTimer += Time.deltaTime;

        if (jumpTimer >= delayBetweenJumps && isGrounded)
        {
            Jump();
            jumpTimer = 0f;
            hasJumped = true;
        }

        // LANDING DETECTION
        if (!wasGrounded && isGrounded && hasJumped)
        {
            Land();
            hasJumped = false;
        }

        wasGrounded = isGrounded;
    }

    private void Jump()
    {
        rigidBody.linearVelocity = new Vector2(
            rigidBody.linearVelocity.x,
            0f
        );

        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Land()
    {
        Instantiate(
            shockwavePrefab,
            shockwaveSpawnPoint.position,
            Quaternion.identity
        );
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
