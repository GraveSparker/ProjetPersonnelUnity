using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;


[RequireComponent(typeof(Controller))]
public class PlayerJump : MonoBehaviour
{

    [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int _maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float _downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, 0.3f)] private float _coyoteTime = 0.2f;
    [SerializeField, Range(0f, 0.3f)] private float _jumpBufferTime = 0.2f;
    [SerializeField] private PlayerMovementState playerMovementState;

    private Controller _controller;
    private Rigidbody2D _rigidBody;
    private CollisionDataRetriever _ground;
    private Vector2 _velocity;

    public float jumpForce { get; private set; }
    private int _jumpPhase;
    private float _jumpSpeed;
    private float _defaultGravityScale;
    private float _coyoteCounter;
    private float _jumpBufferCounter;

    private bool _desiredJump;
    private bool _onGround;
    private bool _isJumping;


    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _ground = GetComponent<CollisionDataRetriever>();
        _controller = GetComponent<Controller>();

        jumpForce = _jumpHeight;
        _defaultGravityScale = _rigidBody.gravityScale;
    }


    void Update()
    {
        _desiredJump = _controller.input.RetrieveJumpInput() || _desiredJump;
    }

    private void FixedUpdate()
    {
        _onGround = _ground.GetOnGround();
        _velocity = _rigidBody.linearVelocity;

        if (_onGround && Mathf.Abs(_rigidBody.linearVelocity.y) < 0.01f)
        {
            _jumpPhase = 0;
            _coyoteCounter = _coyoteTime;
            _isJumping = false;
        }
        else
        {
            _coyoteCounter -= Time.deltaTime;
        }

        if (_desiredJump)
        {
            _desiredJump = false;
            _jumpBufferCounter = _jumpBufferTime;
        }
        else if (!_desiredJump && _jumpBufferCounter > 0)
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_jumpBufferCounter > 0)
        {
            JumpAction();
        }

        if (_controller.input.RetrieveJumpHoldInput() && _rigidBody.linearVelocity.y > 0)
        {
            _rigidBody.gravityScale = _upwardMovementMultiplier;
        }
        else if (!_controller.input.RetrieveJumpHoldInput() || _rigidBody.linearVelocity.y < 0)
        {
            _rigidBody.gravityScale = _downwardMovementMultiplier;
        }
        else if (_rigidBody.linearVelocity.y == 0)
        {
            _rigidBody.gravityScale = _defaultGravityScale;
        }

        if (_onGround && !_isJumping)
        {
            playerMovementState.SetMoveState(PlayerMovementState.MoveState.Idle);
        }


        _rigidBody.linearVelocity = _velocity;
    }


    private void JumpAction()
    {
        if (_coyoteCounter > 0f || (_jumpPhase < _maxAirJumps && _isJumping))
        {
            if (_isJumping)
            {
                _jumpPhase += 1;
            }

            _jumpBufferCounter = 0;
            _coyoteCounter = 0;

            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);
            _isJumping = true;
            playerMovementState.SetMoveState(PlayerMovementState.MoveState.Jump);

            if (_velocity.y > 0f)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
            }
            else if (_velocity.y < 0f)
            {
                _jumpSpeed += Mathf.Abs(_rigidBody.linearVelocity.y);
            }
            _velocity.y += _jumpSpeed;
        }
    }

}