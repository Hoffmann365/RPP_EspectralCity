using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAI : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public int ShadowDmg;
    public int health;
    public float detectionRange = 5f; // Distância em que o inimigo detecta o jogador
    public float attackRange = 1.5f; // Distância em que o inimigo ataca o jogador
    public float moveSpeed = 2f; // Velocidade de movimento do inimigo
    public float attackCooldown = 2f; // Tempo entre ataques
    public float plusDR = 1.5f;
    private bool isAtk = false; // Controle se o inimigo está atacando
    private float lastAttackTime; //Tempo do último ataque
    private float distanceToPlayer;
    private float origDR;
    private float altDR;
    private bool alive = true;
    
    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer shadowSR;
    private BoxCollider2D coll;
    public CircleCollider2D AtkColl;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        shadowSR = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        origDR = detectionRange;
        altDR = detectionRange + plusDR;
        lastAttackTime = -attackCooldown; // Permite atacar imediatamente no início
    }
    
    private void OnEnable()
    {
        GameObserver.DamageOnShadow += Damage;
    }

    private void OnDisable()
    {
        GameObserver.DamageOnShadow -= Damage;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            // Calcula a distância entre o inimigo e o jogador
            distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
            //Desativa o SpriteRenderer e define o colisor como trigger quando o Player está fora da área de detecção
            if (distanceToPlayer > detectionRange)
            {
                shadowSR.enabled = false;
                rig.Sleep();
                coll.isTrigger = true;
                detectionRange = origDR;

            }
            else
            {
                shadowSR.enabled = true;
                coll.isTrigger = false;
                rig.IsAwake();
            }
            if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
            {
                // Se o jogador estiver na área de detecção, mas fora da área de ataque, mover em direção ao jogador
                MoveTowardsPlayer();
            
            }
            else if (distanceToPlayer <= attackRange && !isAtk && Time.time >= lastAttackTime + attackCooldown)
            {
                // Se estiver no range de ataque, e o cooldown já passou, atacar
                StartCoroutine(Attack());
            }
        }
        
        
    }
    
    // Função para mover o inimigo na direção do jogador
    void MoveTowardsPlayer()
    {
        detectionRange = altDR;
        // Calcula a direção do movimento
        Vector2 direction = (player.position - transform.position).normalized;
        
        anim.SetInteger("transition", 1);

        // Move o inimigo na direção do jogador
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    IEnumerator Attack()
    {
        isAtk = true;

        // Inicia a animação de ataque
        anim.SetInteger("transition", 2);

        // Aguarda um pequeno tempo antes de causar dano, simulando o tempo de execução do golpe
        yield return new WaitForSeconds(0.4f);

        // Aplica o dano ao jogador
        GameObserver.OnDamageOnPlayer(ShadowDmg);

        // Define o tempo do último ataque
        lastAttackTime = Time.time;

        // Aguardar o fim da animação e sair do estado de ataque
        yield return new WaitForSeconds(0.2f);

        anim.SetInteger("transition", 1);
        isAtk = false;
    }

    public void Damage(int dmg)
    {
        anim.SetInteger("transition", 1);
        health -= dmg;
        //observer
        anim.SetTrigger("hit");
        //som de hit
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
        alive = false;
        anim.SetTrigger("die");
        Destroy(rig);
        Destroy(coll);
        Destroy(gameObject,0.8f);
    }

    
    // Função para desenhar a área de detecção e ataque no editor (opcional)
    private void OnDrawGizmosSelected()
    {
        // Desenha a área de detecção
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Desenha a área de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    
    
}
