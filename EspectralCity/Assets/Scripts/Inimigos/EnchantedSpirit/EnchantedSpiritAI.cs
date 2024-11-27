using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantedSpiritAI : MonoBehaviour
{
    public Transform[] patrolPoints;  // Pontos de patrulha
    public Transform player;  // Referência ao jogador
    public GameObject projectilePrefab;  // Prefab do projétil
    public Transform firePoint;  // Ponto de spawn do projétil

    public float detectionRange = 10f;  // Distância para detectar o jogador
    public float attackRange = 5f;  // Distância para começar o ataque
    public float moveSpeed = 2f;  // Velocidade de movimento do inimigo
    public float attackCooldown = 2f;  // Tempo entre os ataques
    public float patrolSpeed = 1f;  // Velocidade de patrulha
    private float lastAttackTime = 0f;  // Tempo desde o último ataque
    private bool isPatrolling = true;
    private bool isShooting;

    private int currentPatrolIndex = 0;
    private float distanceToPlayer;
    private Animator anim;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange)
        {
            // Se o jogador estiver fora do alcance de detecção, patrulha entre os pontos
            Patrol();
        }
        else if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            // Se o jogador estiver na área de detecção mas fora do alcance de ataque, mover em direção ao jogador
            MoveTowardsPlayer();
        }
        else if (distanceToPlayer <= attackRange && !isShooting && Time.time >= lastAttackTime + attackCooldown)
        {
            // Se o jogador estiver dentro do alcance de ataque e o cooldown estiver pronto, atacar
            StartCoroutine(Attack());
        }
    }

    void Patrol()
    {
        if (isPatrolling)
        {
            anim.SetInteger("transition", 1);  // Animação de andar
            // Move o inimigo para o próximo ponto de patrulha
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolIndex].position, patrolSpeed * Time.deltaTime);

            // Verifica se chegou no ponto de patrulha atual
            if (Vector2.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.2f)
            {
                // Vai para o próximo ponto de patrulha
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        isPatrolling = false;
        anim.SetInteger("transition", 1);  // Animação de andar

        // Move em direção ao jogador
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    IEnumerator Attack()
    {
        isPatrolling = false;
        isShooting = true;
        anim.SetInteger("transition", 2);  // Animação de ataque
        
        // Aguarda um pouco antes de atirar (simula o tempo da animação de ataque)
        yield return new WaitForSeconds(0.2f);
        
        // Dispara o projétil em direção ao jogador
        ShootProjectile();

        // Define o tempo do último ataque
        lastAttackTime = Time.time;
        
        // Espera o tempo do cooldown antes de permitir outro ataque
        yield return new WaitForSeconds(attackCooldown);
        
        anim.SetInteger("transition", 1);
        
        isShooting = false;
    }

    void ShootProjectile()
    {
        // Instancia o projétil
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Ajuste para verificar a direção do inimigo usando a escala
        bool isFacingRight = transform.localScale.x > 0;

        // Configura a direção do projétil
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 5f;  // Define a velocidade do projétil
    }


    // Função opcional para desenhar os alcances no editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Gizmos.color = Color.green;
        foreach (Transform waypoint in patrolPoints)
        {
            Gizmos.DrawSphere(waypoint.position, 0.2f);  // Desenha uma pequena esfera nos waypoints
        }
    }
}

