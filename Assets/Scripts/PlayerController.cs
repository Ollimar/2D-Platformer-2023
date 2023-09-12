using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float speed = 8f;
    public float jumpPower = 10f;
    private float horizontal;
    private float vertical;
    public Rigidbody2D myRB;
    public Animator myAnim;
    public Animator additionalAnim;
    public bool facingRight = true;
    public bool onMovingPlatform = false;
    public bool onJumpThruPlatform = false;
    public bool onLadder = false;
    public bool climbing = false;
    public bool hasLanded = false;

    public GameObject activeJumpThruPlatform;
    public GameObject activeLadder;

    // Variables for ground check
    public Transform groundCheck;
    public float groundCheckradius = 0.2f;
    public LayerMask groundLayer;

    //References to other scripts
    [SerializeField] private CameraTargetScript cameraTargetScript;
    [SerializeField] private CameraController camController;

    // Screenshake values
    public float jumpFrequency = 3f;
    public float jumpTimer = 0.5f;
    public float jumpAmplitude = 2f;

    // Player death values
    public Transform playerStart;

    public Collider2D myCol;
    public PhysicsMaterial2D slide;
    public PhysicsMaterial2D stop;

    // Coyote Time
    public float coyoteTime = 0.3f;
    public float coyoteTimeTimer;
    public Transform jumpMark;

    // Jump Buffer
    public float jumpBuffer = 0.3f;
    public float jumpBufferTimer;
    

    public void Start()
    {
        transform.position = playerStart.position;
        cameraTargetScript = GameObject.Find("CameraTarget").GetComponent<CameraTargetScript>();
        camController = GameObject.Find("Basic 2D Camera").GetComponent<CameraController>();
        myCol = GetComponent<Collider2D>();
        additionalAnim = GetComponentInChildren<Animator>();
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckradius, groundLayer);       
    }

    public void Jump(InputAction.CallbackContext context)
    {

        if(context.performed)
        {
            jumpBufferTimer = jumpBuffer;       
        }

        
        if(context.performed && climbing)
        {
            myRB.velocity = Vector2.zero;
            myRB.AddForce(Vector2.up * jumpPower/2);
            activeLadder.GetComponent<LadderScript>().StartCoroutine("DetachFromLadders");
        }
        
        
        if(context.canceled && myRB.velocity.y > 0f)
        {
            myRB.velocity = new Vector2(myRB.velocity.x, myRB.velocity.y * 0.4f);
            coyoteTimeTimer = 0f;
        }

        if(context.canceled)
        {
            jumpBufferTimer -= Time.deltaTime;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
    }

    public void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
        facingRight = !facingRight;
        //camController.FlipScreenX(facingRight);
    }

    private void FixedUpdate()
    {

        if(onLadder)
        {
            if(vertical > 0.1)
            {
                climbing = true;
            }
        }

        if(climbing)
        {
            myRB.velocity = new Vector2(0f, vertical*speed);
            myRB.gravityScale = 0f;
        }

        else if(!climbing)
        {
            myRB.velocity = new Vector2(horizontal * speed, myRB.velocity.y);
            myRB.gravityScale = 3.68f;
        }
        

        myAnim.SetFloat("yVelocity", myRB.velocity.y);
        additionalAnim.SetFloat("yVelocity", myRB.velocity.y);

        print(myRB.velocity.x);
    }

    // Update is called once per frame
    void Update()
    {
        additionalAnim.SetBool("onMovingPlatform", onMovingPlatform);

        if(jumpBufferTimer > 0f && coyoteTimeTimer > 0f)
        {
            jumpMark.position = transform.position;
            myRB.AddForce(Vector2.up * jumpPower);
            jumpBufferTimer = 0f;
        }

        if(horizontal != 0f && onMovingPlatform)
        {
            myCol.sharedMaterial = slide;
        }

        if(horizontal == 0f && onMovingPlatform)
        {
            myCol.sharedMaterial = stop;
        }

        if (horizontal < -0.1f && facingRight)
        {
            Flip();
        }

        if (horizontal > 0.1f && !facingRight)
        {
            Flip();
        }

        if(IsGrounded())
        {
            if(!hasLanded)
            {
                additionalAnim.SetBool("Squeeze", true);
                hasLanded = true;
            }
            else if(hasLanded)
            {
                additionalAnim.SetBool("Squeeze", false);
            }

            coyoteTimeTimer = coyoteTime;
            myAnim.SetBool("isGrounded", true);
            
            // Platform snapping
            cameraTargetScript.posY = transform.position.y;
        }

        else
        {
            hasLanded = false;
            coyoteTimeTimer -= Time.deltaTime;
            myAnim.SetBool("isGrounded", false);
        }

        if (horizontal == 0f)
        {
           myAnim.SetBool("isWalking", false);
        }

        else if (horizontal != 0f)
        {
            myAnim.SetBool("isWalking", true);
        }

        if(vertical < -0.1f && onJumpThruPlatform)
        {
            activeJumpThruPlatform.GetComponent<JumpThruPlatform>().StartCoroutine("DropThruPlatform");
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Hazard"))
        {
            camController.ScreenShake(1f,0.5f,1f);
            transform.position = playerStart.position;
        }

        if(collision.gameObject.CompareTag("MovingPlatform"))
        {
            myCol.sharedMaterial = stop;
            onMovingPlatform = true;
        }

        if(collision.gameObject.CompareTag("JumpThruPlatform"))
        {
            onJumpThruPlatform = true;
            activeJumpThruPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("MovingPlatform"))
        {
            myCol.sharedMaterial = slide;
            onMovingPlatform = false;
        }

        if(collision.gameObject.CompareTag("JumpThruPlatform"))
        {
            onJumpThruPlatform = false;           
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("CheckPoint"))
        {
            playerStart.position = collision.transform.position;
        }

        if(collision.gameObject.CompareTag("Ladders"))
        {
            onLadder = true;
            activeLadder = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ladders"))
        {
            onLadder = false;
            climbing = false;
        }
    }
}
