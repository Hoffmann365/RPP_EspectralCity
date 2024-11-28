using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
    private Vector3 patrolPointA;  // Ponto inicial de patrulha
    private Vector3 patrolPointB;  // Ponto final de patrulha
    public float patrolDistance = 5f;
    public float speed = 2f;        // Velocidade de patrulha
    public int damage = 10;         // Dano causado ao jogador
    public float attackCooldown = 1f; // Tempo entre ataques
    private bool isAtk;
    public int health = 10;
    private bool alive = true;
    
    
    private Vector3 currentTarget;  // Alvo atual para a patrulha
    private float lastAttackTime = 0f; // Tempo do último ataque
    
    private Rigidbody2D rig;
    private Animator anim;
    private BoxCollider2D coll;
    
    private AtkRange atkRange;

    private void Start()
    {
        // Define os pontos de patrulha baseados na posição inicial
        patrolPointA = transform.position - Vector3.right * (patrolDistance / 2);
        patrolPointB = transform.position + Vector3.right * (patrolDistance / 2);
        currentTarget = patrolPointA; // Começa indo para o ponto A
        
        rig = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        
        atkRange = GetComponentInChildren<AtkRange>();
    }

    private void Update()
    {
        if (alive)
        {
            // Se o jogador está no alcance de ataque, ataca
            if (atkRange != null && atkRange.playerInRange)
            {
                Attack();
            }
            else
            {
                Patrol(); // Caso contrário, patrulha entre os pontos
            }
        }
    }

    private void Patrol()
    {
        anim.SetInteger("transition", 1);
        // Move o inimigo na direção do alvo atual
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        // Verifica se chegou no ponto de patrulha
        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            // Alterna o alvo entre os pontos de patrulha
            currentTarget = currentTarget == patrolPointA ? patrolPointB : patrolPointA;

            // Inverte a direção do inimigo
            Flip();
        }
    }

    private void Attack()
    {
        // Verifica se está no cooldown do ataque
        if (Time.time - lastAttackTime >= attackCooldown && !isAtk)
        {
            // Aqui você pode adicionar animações ou efeitos de ataque
            StartCoroutine(Atk());

        }
    }
    
    IEnumerator Atk()
    {
        isAtk = true;

        // Inicia a animação de ataque
        anim.SetInteger("transition", 2);

        // Aguarda um pequeno tempo antes de causar dano, simulando o tempo de execução do golpe
        yield return new WaitForSeconds(0.5f);

        // Aplica o dano ao jogador
        GameObserver.OnDamageOnPlayer(damage);

        // Define o tempo do último ataque
        lastAttackTime = Time.time;

        // Aguardar o fim da animação e sair do estado de ataque
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Esqueleto atacou!");

        anim.SetInteger("transition", 1);
        isAtk = false;
    }

    private void Flip()
    {
        // Inverte o eixo X do inimigo para simular a mudança de direção
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    
    public void Damage(int dmg)
    {
        health -= dmg;
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
        anim.SetInteger("transition", 1);
        
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
        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmos()
    {
        // Desenha linhas conectando os pontos de patrulha no editor
        Gizmos.color = Color.red;
        if (patrolPointA != null && patrolPointB != null)
        {
            // Ponto inicial e final (calculado dinamicamente)
            Vector3 start = transform.position - Vector3.right * (patrolDistance / 2);
            Vector3 end = transform.position + Vector3.right * (patrolDistance / 2);

            // Desenha linha de patrulha
            Gizmos.DrawLine(start, end);

            // Desenha esferas nos pontos
            Gizmos.DrawSphere(start, 0.2f);
            Gizmos.DrawSphere(end, 0.2f);
        }
    }
}
