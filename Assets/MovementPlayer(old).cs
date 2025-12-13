//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class PlayerMovement : MonoBehaviour
//{

//    public Rigidbody2D body;

//    public BoxCollider2D groundCheck;

//    public LayerMask groundMask;


//    [Range(0f, 1f)]
//    public float groundDecay;

//    public float acceleration;

//    public float maxXSpeed;



//    public float jumpSpeed;


//    public bool grounded;

//    float xinput;

//    float yinput;

  

//     Update is called once per frame
    
//  void Update()
//  {
//       CheckInput();
//        HandleJump();
//   }


//     void FixedUpdate()
//    {
//        CheckGround();
//        ApplyFriction();
//        HanldeXMovement();
//    }


//    void CheckInput()
//    {
//        xinput = Input.GetAxis("Horizontal");
//        yinput = Input.GetAxis("Vertical");
//    }

//    void HanldeXMovement()
//    {
//        if (Mathf.Abs(xinput) > 0)
//        {

//            //increment velocity by our acceleration, then clamp within max
//            float increment = xinput * acceleration;
//            float newSpeed = Mathf.Clamp(body.linearVelocity.x + increment, -maxXSpeed, maxXSpeed);
//            body.linearVelocity = new Vector2(newSpeed, body.linearVelocity.y);

//            FaceInput();
//        }

        
//    }

//    void FaceInput()
//    {
//        float direction = Mathf.Sign(xinput);
//        transform.localScale = new Vector3(direction, 1, 1);
//    }


//    void HandleJump()
//    {
//        if (Input.GetButtonDown("Jump") && grounded)
//        {
//            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
//        }
//    }


//    void CheckGround()
//    {
//        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0; 

//    }

//    void ApplyFriction()
//    {
//        if (grounded && xinput == 0 && body.linearVelocity.y <= 0)
//        {
//            body.linearVelocity *= groundDecay;
//        }
//    }

//}
