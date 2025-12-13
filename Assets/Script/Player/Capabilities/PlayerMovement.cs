using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private InputController input = null;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D rigidBody;
    private CollisionDataRetriever ground;
    private PlayerJump jump;
    public PlayerState pState;
    public static PlayerMovement Instance;

    private float maxSpeedChange;
    private float acceleration;
    private float xPosLastFrame;
    private bool onGround;


 
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        rigidBody = GetComponent<Rigidbody2D>();
        ground = GetComponent<CollisionDataRetriever>();
        jump = GetComponent<PlayerJump>();
        pState = GetComponent<PlayerState>();
    }

 
    void Update()
    {
        if (pState.cutscene) return;

        direction.x = input.RetrieveMoveInput();
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);

    }

    private void FixedUpdate()
    {
        if (pState.cutscene) return;

        onGround = ground.GetOnGround();
        velocity = rigidBody.linearVelocity;

        acceleration = onGround ? maxAcceleration: maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        FlipCharacterX();

        rigidBody.linearVelocity = velocity;
    }

    void FlipCharacterX()
    {
        if (direction.x > 0 && (transform.position.x > xPosLastFrame))
        {
            transform.eulerAngles = new Vector2(0, 0);
            pState.lookingRight = true;
        }
        else if (direction.x < 0 && (transform.position.x < xPosLastFrame))
        {
            transform.eulerAngles = new Vector2(0, 180);
            pState.lookingRight = false;
        }

        xPosLastFrame = transform.position.x;
    }

    public void KnockbackPlayer (Vector2 knockbackForce, int direction)
    {
        knockbackForce.x *= direction;
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        rigidBody.AddForce(knockbackForce, ForceMode2D.Impulse);
    }

    public IEnumerator WalkIntoNewScene(Vector2 _exitDir, float _delay)
    {
        Physics2D.IgnoreLayerCollision(6, 8, true);
        if (_exitDir.y > 0)
        {
            rigidBody.linearVelocity = jump.jumpForce * _exitDir;
        }

        if (_exitDir.x != 0)
        {
            direction.x = _exitDir.x > 0 ? 1 : -1;

            direction.x = input.RetrieveMoveInput();
        }

        FlipCharacterX();
        yield return new WaitForSeconds(_delay);
        Physics2D.IgnoreLayerCollision(6, 8, false);
        pState.cutscene = false;
    }
}
