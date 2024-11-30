using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public Transform ceilingPoint; // Ponto no teto onde o morcego retorna quando adormecido
    public int batDmg;
    public int health;
    public float detectionRange = 5f; // Distância em que o morcego detecta o jogador
    public float attackRange = 1.5f; // Distância em que o morcego ataca o jogador
    public float moveSpeed = 2f; // Velocidade de movimento do morcego
    public float attackCooldown = 2f; // Tempo entre ataques
    public float hoverSpeed = 1f; // Velocidade de flutuação ao atacar
    public float hoverAmplitude = 0.5f; // Amplitude de flutuação ao atacar

    private bool isAtk = false; // Controle se o morcego está atacando
    private bool isAwake = false; // Controle se o morcego está acordado
    private bool alive = true;
    private float lastAttackTime; // Tempo do último ataque

    private Animator anim;
    private SpriteRenderer batSR;
    private Rigidbody2D rig;

    // Start é chamado antes da primeira atualização
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        batSR = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        lastAttackTime = -attackCooldown; // Permite atacar imediatamente no início
        transform.position = ceilingPoint.position; // Inicia no teto
    }

    void Update()
    {
        if (alive)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange && !isAwake)
            {
                // Se o jogador estiver dentro do alcance de detecção, acorda o morcego
                WakeUp();
            }
            else if (distanceToPlayer > detectionRange && isAwake)
            {
                // Se o jogador sair do alcance, o morcego retorna ao teto
                GoToSleep();
            }

            if (isAwake && distanceToPlayer > attackRange)
            {
                // Move-se na direção do jogador
                MoveTowardsPlayer();
            }
            else if (isAwake && distanceToPlayer <= attackRange && !isAtk && Time.time >= lastAttackTime + attackCooldown)
            {
                // Ataca o jogador
                StartCoroutine(Attack());
            }
        }
    }

    void WakeUp()
    {
        isAwake = true;
        anim.SetTrigger("wake");
    }

    void GoToSleep()
    {
        isAwake = false;
        anim.SetTrigger("sleep");
        transform.position = Vector3.MoveTowards(transform.position, ceilingPoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, ceilingPoint.position) < 0.1f)
        {
            // Se chegou ao teto, redefine a posição exata
            transform.position = ceilingPoint.position;
        }
    }

    void MoveTowardsPlayer()
    {
        anim.SetInteger("transition", 1);

        // Calcula a direção do movimento
        Vector2 direction = (player.position - transform.position).normalized;

        // Adiciona flutuação ao movimento do morcego
        float hover = Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
        Vector3 hoverOffset = new Vector3(0, hover, 0);

        // Move o morcego na direção do jogador
        transform.position = (Vector3)Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime) + hoverOffset;

    }

    IEnumerator Attack()
    {
        isAtk = true;

        // Inicia a animação de ataque
        anim.SetInteger("transition", 2);

        // Aguarda um pequeno tempo antes de causar dano
        yield return new WaitForSeconds(0.4f);

        // Aplica o dano ao jogador
        GameObserver.OnDamageOnPlayer(batDmg);

        // Define o tempo do último ataque
        lastAttackTime = Time.time;

        // Aguarda o fim da animação
        yield return new WaitForSeconds(0.2f);

        anim.SetInteger("transition", 1);
        isAtk = false;
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        anim.SetTrigger("hit");

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
        Destroy(gameObject, 0.8f);
    }

    // Desenha as áreas de detecção e ataque no editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
