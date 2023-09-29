using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float enemyRunSpeed = 1.0f;
    Rigidbody2D enemyRigidBody;
    void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFacingLeft()) 
        {
        enemyRigidBody.velocity = new Vector2 (-enemyRunSpeed, 0f);
        }
        else
        {
            enemyRigidBody.velocity = new Vector2(enemyRunSpeed, 0f);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FlipSprite();
    }

    private void FlipSprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(enemyRigidBody.velocity.x), 1f);
    }

    private bool IsFacingLeft()
    {
        return transform.localScale.x > 0;
    }    
}
