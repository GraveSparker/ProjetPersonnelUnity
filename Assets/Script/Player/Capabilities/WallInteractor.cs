//using UnityEngine;

//public class WallInteractor : MonoBehaviour
//{
//    public bool WallJumping {  get; private set; }

//    [Header("Wall Slide")]
//    [SerializeField][Range(0.1f, 5f)] private float _wallSlideMaxSpeed =2f;
//    [Header("Wall Jump")]
//    [SerializeField] private Vector2 _wallJumpClimb = new Vector2(4f, 12f);
//    [SerializeField] private Vector2 _wallJumpBounce = new Vector2(10.7f, 10f);

//    private CollisionDataRetriever _collisionDataRetriever;
//    private Rigidbody2D _body;
//    private Controller _controller;

//    private Vector2 _velocity;
//    private bool _onWall;
//    private bool _onGround;
//    private bool _desiredJump;

//    private float _wallDirectionX;

//    void Start()
//    {
//        _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
//        _body = GetComponent<Rigidbody2D>();
//        _controller = GetComponent<Controller>();
//    }


//    void Update()
//    {
//        _desiredJump |= _controller.input.RetrieveJumpInput();
//    }


//    private void FixedUpdate()
//    {
//        _velocity = _body.linearVelocity;
//        _onWall = _collisionDataRetriever.OnWall;
//        _onGround = _collisionDataRetriever.OnGround;
//        _wallDirectionX = _collisionDataRetriever.ContactNormal.x;

//        #region Wall Slide
//        if(_onWall )
//        {
//            if(_velocity.y < -_wallSlideMaxSpeed)
//            {
//                _velocity.y = -_wallSlideMaxSpeed;
//            }
//        }
//        #endregion

//        #region Wall Jump

//        if((_onWall && _velocity.x == 0) || _onGround)
//        {
//            WallJumping = false;
//        }

//        if(_desiredJump)
//        {
//            if(-_wallDirectionX == _controller.input.RetrieveMoveInput())
//            {
//                _velocity = new Vector2(_wallJumpClimb.x * _wallDirectionX, _wallJumpClimb.y);
//                WallJumping = true;
//                _desiredJump = false;
//            }
//            else if(_controller.input.RetrieveMoveInput() == 0)
//            {
//                _velocity = new Vector2(_wallJumpBounce.x * _wallDirectionX, _wallJumpBounce.y);
//                WallJumping = true;
//                _desiredJump = false;
//            }
//        }
//        #endregion

//        _body.linearVelocity = _velocity;
//    }

   
//}
