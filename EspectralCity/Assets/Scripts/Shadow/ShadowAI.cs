using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAI : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public float detectionRange = 5f; // Distância em que o inimigo detecta o jogador
    public float attackRange = 1.5f; // Distância em que o inimigo ataca o jogador
    public float moveSpeed = 2f; // Velocidade de movimento do inimigo
    public float attackCooldown = 2f; // Tempo entre ataques
    public float plusDR = 1.5f;
    public static float movement;
    private bool isAtk = false; // Controle se o inimigo está atacando
    private float distanceToPlayer;
    private float origDR;
    private float altDR;
    
    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer shadowSR;
    private CircleCollider2D coll;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        coll = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        shadowSR = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        origDR = detectionRange;
        altDR = detectionRange + plusDR;
    }

    // Update is called once per frame
    void Update()
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
        else if (distanceToPlayer <= attackRange && !isAtk)
        {
            // Se o jogador estiver dentro da área de ataque, realizar o ataque
            StartCoroutine(Attack());
        }
        
    }
    
    // Função para mover o inimigo na direção do jogador
    void MoveTowardsPlayer()
    {
        movement = Input.GetAxis("Horizontal");
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

        // Aqui você pode adicionar o código para a animação de ataque
        anim.SetInteger("transition", 2);

        // Aguarda pelo tempo de cooldown do ataque
        yield return new WaitForSeconds(attackCooldown);
        anim.SetInteger("transition", 1);
        isAtk = false;
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
