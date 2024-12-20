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
    public int health;
    public bool onAir;
    public bool hasKey = false;
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
    private void OnEnable()
    {
        GameObserver.DamageOnPlayer += Damage;
        
    }

    private void OnDisable()
    {
        GameObserver.DamageOnPlayer -= Damage;
    }

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        feet = GetComponent<CircleCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Key"))
        {
            CollectablesObserver.OnAddKeysEvent();
            Destroy(collision.gameObject); // Remove a chave do jogo
        }

        if (collision.CompareTag("Fall"))
        {
            rig.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            Damage(100);
        }
    }

    private void OnTriggerStay2D(Collider2D feet)
    {
        if (feet.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            onAir = false;
            isJumping = false;
        }
        
    }

    private void OnTriggerExit2D(Collider2D feet)
    {
        if (feet.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            onAir = true;
        }
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
        if (Input.GetButtonDown("Jump") && !onAir) // Verifica se o jogador está no chão
        {
            isJumping = true; // Define que o jogador está pulando
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Aplica a força para o pulo
            anim.SetInteger("transition", 2); // Define a animação de pulo
        
            AudioObserver.OnPlaySfxEvent("pulo"); // Reproduz o som do pulo
            ParticleObserver.OnParticleSpawnEvent(transform.position); // Gera partículas no local do jogador
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
                yield return new WaitForSeconds(0.8f);
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
                yield return new WaitForSeconds(1.4f);
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

    void Damage(int dmg)
    {
        anim.SetBool("meleeATK", false);
        anim.SetBool("rangedATK", false);
        anim.SetInteger("transition", 0);
        health -= dmg;
        //atualizar barra de vida
        anim.SetTrigger("hit");
        AudioObserver.OnPlaySfxEvent("hit");
        if (transform.eulerAngles.y == 0)
        {
            float knockbackDirection = transform.eulerAngles.y == 0 ? -1 : 1;
            transform.position += new Vector3(0.5f * knockbackDirection, 0, 0);
        }

        if (transform.eulerAngles.y == 180)
        {
            float knockbackDirection = transform.eulerAngles.y == 0 ? -1 : 1;
            transform.position += new Vector3(0.5f * knockbackDirection, 0, 0);
        }

        if (health <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        anim.SetTrigger("die");
        Destroy(rig);
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(feet);
        Destroy(gameObject,0.8f);
        GameObserver.OnGameOver();
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
