using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public bool onAir;
    
    public static float movement;
    
    private bool isJumping;
    private bool doublejump;
    
    private Rigidbody2D rig;
    private Animator anim;
    private CircleCollider2D feet;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        feet = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D feet)
    {
        onAir = false;
        isJumping = false;
    }

    private void OnTriggerExit2D(Collider2D feet)
    {
        onAir = true;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0)
        {
            if (!isJumping && !onAir)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (movement < 0)
        {
            if (!isJumping && !onAir)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !onAir && !isJumping)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !onAir)
        {
            if (!isJumping)
            {
                isJumping = true;
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                anim.SetInteger("transition", 2);

                doublejump = true;
            }
            
        }
        else if (Input.GetButtonDown("Jump") && doublejump)
        {
            anim.SetInteger("transition", 2);
            rig.AddForce(new Vector2(0, jumpForce * 1), ForceMode2D.Impulse);
            doublejump = false;
        }
    }
}
