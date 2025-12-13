using System;
using UnityEngine;

public class PlayerMovementState : MonoBehaviour
{
    public enum MoveState
    {
        Idle,
        Run,
        Jump,
        Fall,
        Attack,
        Hurt,
        Death
    }

    public MoveState CurrentMoveState { get; private set; }

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;

    [Header("Movement Settings")]
    [SerializeField] private float runThreshold = 0.4f;  // Minimum x velocity to consider running
    [SerializeField] private float idleDelay = 0.15f;    // Delay before switching to idle

    private float idleDelayTimer = 0f;

    private const string idleAnim = "PlayerIdle";
    private const string runAnim = "PlayerRun";
    private const string jumpAnim = "PlayerJump";
    private const string fallAnim = "PlayerFall";
    private const string attackAnim = "PlayerAttack";
    private const string hurtAnim = "PlayerHurt";
    private const string deathAnim = "PlayerDeath";
    public static Action<MoveState> OnPlayerMoveStateChanged;

    private void Update()
    {
        Vector2 vel = rigidBody.linearVelocity;

        // Clamp tiny velocities to zero
        if (Mathf.Abs(vel.x) < 0.05f) vel.x = 0f;
        if (Mathf.Abs(vel.y) < 0.05f) vel.y = 0f;

        float moveX = Mathf.Abs(vel.x);

        // -------------------- MOVEMENT STATES --------------------
        if (vel.y > 0.1f)
        {
            SetMoveState(MoveState.Jump);
        }
        else if (vel.y < -0.1f)
        {
            SetMoveState(MoveState.Fall);
        }
        else if (moveX > runThreshold)
        {
            idleDelayTimer = idleDelay;
            SetMoveState(MoveState.Run);
        }
        else
        {
            // Player slowing down -> apply delay before idle
            idleDelayTimer -= Time.deltaTime;
            if (idleDelayTimer <= 0f)
            {
                SetMoveState(MoveState.Idle);
            }
        }
    }

    public void SetMoveState(MoveState moveState)
    {
        if (moveState == CurrentMoveState) return;

        switch (moveState)
        {
            case MoveState.Idle:
                animator.Play(idleAnim);
                break;
            case MoveState.Run:
                animator.Play(runAnim);
                break;
            case MoveState.Jump:
                animator.Play(jumpAnim);
                break;
            case MoveState.Fall:
                animator.Play(fallAnim);
                break;
            case MoveState.Attack:
                animator.Play(attackAnim);
                break;
            case MoveState.Hurt:
                animator.Play(hurtAnim);
                break;
            case MoveState.Death:
                animator.Play(deathAnim);
                break;
        }

        OnPlayerMoveStateChanged?.Invoke(moveState);
        CurrentMoveState = moveState;
    }
}
