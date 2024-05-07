using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] LayerMask groundMask;
    
    [Header("Move")] 
    [SerializeField] float maxspeed; 
    [SerializeField] float acceleration, decceleration, velPow,friction;
    Vector2 moveForceDir;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float coyoteTime, jumpBufferTime, jumpCutValue, gravityScaleValue, gravityScaleMultiplier;
    
    [Header("Slope")]
    [SerializeField] float slopeCheckDistance, slopeSpeedDecrease;
    bool isOnSlope;
    [Header("PhysicsMaterial")]

    [SerializeField] PhysicsMaterial2D slippyMaterial, frictionMaterial;

    Animator animator;
    [HideInInspector] public Rigidbody2D rb;
    CapsuleCollider2D playerCollider;
    [HideInInspector] public bool jumpCmd, isOnGround;
    float xIn;
    float lastGroundTime, lastJumpTime;

    // Start is called before the first frame update
    void Start()
    {
        isOnGround = true;
        jumpCmd = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xIn = Input.GetAxis("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumpCmd = true;
        }
    }

    void Move()
    {
        float targetSpeed = isOnSlope? xIn * maxspeed * slopeSpeedDecrease : xIn * maxspeed;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) >= 0.01f)? acceleration : decceleration;
        float moveForce = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPow) * Mathf.Sign(speedDif);
        
        if(targetSpeed == 0 && rb.velocity.x < 0.01f)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
           //Debug.Log("set0");
        }
        else
        {
            rb.AddForce(moveForce * moveForceDir.normalized);   
            // Debug.Log(moveForce);  
        } 

        //flip
        if(xIn != 0)
        {
            if(xIn > 0)
                this.transform.localScale = new Vector3(  1 , transform.localScale.y, 1);
            else
                this.transform.localScale = new Vector3(  -1 , transform.localScale.y, 1);
        }

        //friction

        if(isOnGround && xIn == 0)
        {
            float frictionForce = Mathf.Min(Mathf.Abs(rb.velocity.magnitude), friction);
            frictionForce *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(moveForceDir * -frictionForce, ForceMode2D.Impulse); 
        }

    }
    void Jump()
    {
        if(jumpCmd)
        {
            jumpCmd = false;
            lastJumpTime = jumpBufferTime;
        }
        else
        {
            lastJumpTime -= Time.deltaTime;   
        }

        if(lastGroundTime > 0 && lastJumpTime > 0)
        {
            lastJumpTime = 0;
            animator.SetTrigger("jump");
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        

        //JumpCut
        if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(jumpCutValue * Vector2.down, ForceMode2D.Impulse);
        }
        //FallGravity
        if(rb.velocity.y < 0 && !isOnGround)
        {
            rb.gravityScale = gravityScaleValue * gravityScaleMultiplier;
        }
        else 
        {
            rb.gravityScale = gravityScaleValue;
        }
    }

    bool CheckGroundFun()
    {
        RaycastHit2D hit = Physics2D.CapsuleCast(playerCollider.bounds.center, playerCollider.size, CapsuleDirection2D.Vertical, 0, Vector2.down, 0.05f, groundMask);
        if(hit.collider != null)
        {
            lastGroundTime = coyoteTime;
            animator.SetBool("isOnGround", true);
            return true;
        }
        else
        {
            lastGroundTime -= Time.deltaTime;
            animator.SetBool("isOnGround", false);
            return false;
        }
    }

    void SlopeCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, playerCollider.size.y / 2, 0), Vector2.down, slopeCheckDistance, groundMask);
        if(hit.collider != null)
        {

            if(Vector2.Angle(hit.normal, Vector2.up) > 10f  && Vector2.Angle(hit.normal, Vector2.up) < 60 && isOnGround)
            {
                moveForceDir = Vector2.Perpendicular(hit.normal) * -1;
                isOnSlope = true;
                
                if(xIn == 0)
                    rb.sharedMaterial = frictionMaterial;   
                else
                    rb.sharedMaterial = slippyMaterial;
            }
            else
            {
                moveForceDir = Vector2.right;
                rb.sharedMaterial = slippyMaterial;
                isOnSlope = false;
            }
                

            Debug.DrawRay(hit.point, hit.normal, Color.red);
            // Debug.Log(Vector2.Angle(hit.normal, Vector2.up));
        }
        else
        {
            moveForceDir = Vector2.right;
            rb.sharedMaterial = slippyMaterial;
            isOnSlope = false;
        }
    }

    void FixedUpdate() 
    {
        isOnGround = CheckGroundFun();
        SlopeCheck();
        Move();
        Jump();
        animator.SetFloat("speed",Mathf.Abs(rb.velocity.x));
    }


    private void OnDisable() 
    {
        animator.SetFloat("speed", 0);
        animator.SetBool("isOnGround", true);
        rb.velocity = Vector2.zero;
    }
}
