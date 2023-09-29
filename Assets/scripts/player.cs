using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class player : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float climbingSpeed = 8f;
    [SerializeField] Vector2 hitKick =  new Vector2 (50f, 50f);

    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    BoxCollider2D myBoxCollider2D;
    PolygonCollider2D myPlayersFeet;

    float strartingGravityScale;
    bool isHurting = false;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myPlayersFeet = GetComponent<PolygonCollider2D>();

        strartingGravityScale = myRigidBody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isHurting)
        {
            Run();
            Jump();
            Climb();
            if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
            {
                PlayerHit();
            }

        }
        
    }

    private void PlayerHit()
    {
       myRigidBody2D.velocity = hitKick * new Vector2(-transform.localScale.x, 1f);
        myAnimator.SetTrigger("Hitting");
        isHurting = true;
    }
    IEnumerator stopHurting() 
    { 
        yield return new WaitForSeconds(2f);
        isHurting =false;
        StartCoroutine(stopHurting());
    }

    private void Climb()
    {
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            float controlthrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbingVelocity = new Vector2(myRigidBody2D.velocity.x, controlthrow * climbingSpeed);
            myRigidBody2D.velocity = climbingVelocity;
            myAnimator.SetBool("Climbing",true);
            myRigidBody2D.gravityScale = 0f;
        }
        else
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody2D.gravityScale = strartingGravityScale;

        }
    }

    private void Jump()
    {
        if (!myPlayersFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        bool isJumping = CrossPlatformInputManager.GetButtonDown("Jump");
        if (isJumping)
        {
            Vector2 jumpVelocity = new Vector2(myRigidBody2D.velocity.x,jumpSpeed);
            myRigidBody2D.velocity = jumpVelocity;
        }
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");

        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody2D.velocity.y);
        myRigidBody2D.velocity = playerVelocity;
        FlipSprite();
        ChangingToRunningState();
    }

    private void ChangingToRunningState()
    {
        bool runningHozirontaly = Mathf.Abs(myRigidBody2D.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Runing", runningHozirontaly);
    }

    private void FlipSprite()
    {
        bool runningHozirontaly = Math.Abs(myRigidBody2D.velocity.y) > Mathf.Epsilon;
        
        if (runningHozirontaly )
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.velocity.x),1f);
        }
    }
     
}
