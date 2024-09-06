using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSkills
{
    melee,
    ranged
}

public class Player : MonoBehaviour
{
    public PlayerSkills currentWeapon = PlayerSkills.melee;
    public float speed;
    public float jumpForce;
    public bool onAir;
    public GameObject bullet;
    public Transform firePoint;
    
    public static float movement;
    
    private bool isJumping;
    private bool isAtk;
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
        WeaponSwitch();
        Attack();
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

        if (movement == 0 && !onAir && !isJumping && !isAtk)
        {
            anim.SetInteger("transition", 0);
        }

        if (isAtk == true && !onAir)
        {
            movement = 0;
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

    void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentWeapon == PlayerSkills.melee)
            {
                currentWeapon = PlayerSkills.ranged;
            }
            else if (currentWeapon == PlayerSkills.ranged)
            {
                currentWeapon = PlayerSkills.melee;
            }
        }
    }

    

    
     void Attack()
    {
        StartCoroutine("Atk");
    }
     
     IEnumerator Atk()
    {
        if (currentWeapon == PlayerSkills.melee)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isAtk = true;
                anim.SetBool("meleeATK", true);
                anim.SetInteger("transition", 3);
                yield return new WaitForSeconds(0.2f);
                isAtk = false;
            }
        }
        else if (currentWeapon == PlayerSkills.ranged)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isAtk = true;
                anim.SetBool("rangedATK", true);
                anim.SetInteger("transition", 4);
                yield return new WaitForSeconds(2f);
                isAtk = false;
            }
        }
    }

    void EndAnimationATK()
    {
        anim.SetBool("meleeATK", false);
        anim.SetBool("rangedATK", false);
        anim.SetInteger("transition", 0);
    }
    
    void InstBullet()
    {
        GameObject Bullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        if (transform.rotation.y == 0)
        {
            Bullet.GetComponent<PlayerShot>().isRight = true;
        }
        if (transform.rotation.y == 180)
        {
            Bullet.GetComponent<PlayerShot>().isRight = false;
        }
    }
    
}
