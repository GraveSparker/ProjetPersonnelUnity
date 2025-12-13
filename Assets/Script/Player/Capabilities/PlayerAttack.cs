using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField] private Transform sideAttackTransform;
    [SerializeField] private Transform upAttackTransform;
    [SerializeField] private Transform downAttackTransform;

    [SerializeField] private Vector2 sideAttackArea;
    [SerializeField] private Vector2 upAttackArea;
    [SerializeField] private Vector2 downAttackArea;

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private PlayerMovementState playerMovementState;

    [SerializeField] private int damage = 1;
    [SerializeField] private float timeBetweenAttack = 0.3f; // cooldown between attacks

    [SerializeField] private int recoilXSteps = 5;
    [SerializeField] private int recoilYSteps = 5;
    [SerializeField] private float recoilXSpeed = 100;
    [SerializeField] private float recoilYSpeed = 100;

    private CollisionDataRetriever ground;
    public PlayerState pState;
    private bool onGround;
    private float yAxis;
    private float timeSinceAttack;
    private float gravity;
    private int stepsXRecoiled;
    private int stepsYRecoiled;
    private HashSet<Enemy> enemiesHitThisAttack = new HashSet<Enemy>();


    private void Awake()
    {
        ground = GetComponent<CollisionDataRetriever>();
        pState = GetComponent<PlayerState>();

        gravity = rigidBody.gravityScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (sideAttackTransform) Gizmos.DrawWireCube(sideAttackTransform.position, sideAttackArea);
        if (upAttackTransform) Gizmos.DrawWireCube(upAttackTransform.position, upAttackArea);
        if (downAttackTransform) Gizmos.DrawWireCube(downAttackTransform.position, downAttackArea);
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        yAxis = input.RetrieveUpDownInput();

        // Attack only when input is pressed AND cooldown is ready
        if (input.RetrieveAttackInput() && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0;
            enemiesHitThisAttack.Clear(); // reset hits for new swing
            PerformAttack();
            StartCoroutine(AttackStateLock());
        }
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();

        if (pState.recoilingX || pState.recoilingY)
        {
            Recoil();
        }
    }

    private IEnumerator AttackStateLock()
    {
        playerMovementState.SetMoveState(PlayerMovementState.MoveState.Attack);

        // Prevent movement changing animation
        yield return new WaitForSeconds(0.25f);

        // After attack finishes, allow normal states
        playerMovementState.SetMoveState(PlayerMovementState.MoveState.Idle);
    }

    void PerformAttack()
    {
        // Choose attack direction based on input
        if (yAxis > 0)
        {
            Hit(upAttackTransform, upAttackArea, ref pState.recoilingY, recoilYSpeed);
            SlashEffectAtAngle(slashEffect, 90, upAttackTransform);
        }
        else if (yAxis < 0 && !onGround)
        {
            Hit(downAttackTransform, downAttackArea, ref pState.recoilingY, recoilYSpeed);
            SlashEffectAtAngle(slashEffect, -90, downAttackTransform);
        }
        else
        {
            Hit(sideAttackTransform, sideAttackArea, ref pState.recoilingX, recoilXSpeed);
            Instantiate(slashEffect, sideAttackTransform);
        }
    }

    void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, enemyLayer);

        if (objectsToHit.Length > 0)
        {
            pState.recoilingX = true;
            pState.recoilDirectionX = (pState.lookingRight ? -1 : 1);

            pState.recoilingY = true;
            pState.recoilDirectionY = (yAxis < 0 ? 1 : -1);

        }

        for (int i = 0; i < objectsToHit.Length; i++)
        {
            Enemy enemy = objectsToHit[i].GetComponent<Enemy>();

            if (enemy != null && !enemiesHitThisAttack.Contains(enemy))
            {
                enemiesHitThisAttack.Add(enemy);

                // direction from player to enemy (so knockback pushes enemy away)
                Vector2 knockDir = (objectsToHit[i].transform.position - transform.position).normalized;

                // Call your EnemyHit with (damage, direction, force)
                enemy.EnemyHit(damage, knockDir, _recoilStrength);

                Debug.Log("Hit: " + objectsToHit[i].name);
            }
        }
    }


    private void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        GameObject effect = Instantiate(_slashEffect, _attackTransform.position, Quaternion.identity);
        effect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        effect.transform.SetParent(_attackTransform);
    }

    void Recoil()
    {
        if (pState.recoilingX)
        {
            if (pState.lookingRight)
            {
                rigidBody.linearVelocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rigidBody.linearVelocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if (pState.recoilingY)
        {
            rigidBody.gravityScale = 0;
            if (yAxis < 0)
            {
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, recoilYSpeed);
            }
            else
            {
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, -recoilYSpeed);
            }
        }
        else
        {
            rigidBody.gravityScale = gravity;
        }

        //stop recoil
        if (pState.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
        if (pState.recoilingY && stepsYRecoiled < recoilYSteps)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }

        if (onGround)
        {
            StopRecoilY();
        }
    }


    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }

    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pState.recoilingY = false;
    }
}
