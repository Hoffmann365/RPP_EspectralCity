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
    public float attackRange = 1.5f; // Distância de alcance para o ataque
    public int damage = 10;         // Dano causado ao jogador
    public float attackCooldown = 1f; // Tempo entre ataques
    public Transform player;        // Referência ao jogador
    private bool isAtk;
    
    private Vector3 currentTarget;  // Alvo atual para a patrulha
    private float lastAttackTime = 0f; // Tempo do último ataque
    private Animator anim;

    private void Start()
    {
        // Define os pontos de patrulha baseados na posição inicial
        patrolPointA = transform.position - Vector3.right * (patrolDistance / 2);
        patrolPointB = transform.position + Vector3.right * (patrolDistance / 2);
        currentTarget = patrolPointA; // Começa indo para o ponto A
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Se o jogador está no alcance de ataque, ataca
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Attack();
        }
        else
        {
            Patrol(); // Caso contrário, patrulha entre os pontos
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
            Debug.Log("Esqueleto atacou!");
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

        // Desenha o alcance de ataque no editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
