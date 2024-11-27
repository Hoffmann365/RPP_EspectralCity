using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
    public Transform patrolPointA;  // Ponto inicial de patrulha
    public Transform patrolPointB;  // Ponto final de patrulha
    public float speed = 2f;        // Velocidade de patrulha
    public float attackRange = 1.5f; // Distância de alcance para o ataque
    public int damage = 10;         // Dano causado ao jogador
    public float attackCooldown = 1f; // Tempo entre ataques
    public Transform player;        // Referência ao jogador

    private Transform currentTarget;  // Alvo atual para a patrulha
    private float lastAttackTime = 0f; // Tempo do último ataque

    private void Start()
    {
        // Define o alvo inicial de patrulha
        currentTarget = patrolPointA;
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
        // Move o inimigo na direção do alvo atual
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // Verifica se chegou no ponto de patrulha
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
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
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // Aqui você pode adicionar animações ou efeitos de ataque
            Debug.Log("Esqueleto atacou!");

            // Aplica dano ao jogador (substitua pela lógica do seu jogador)
            GameObserver.OnDamageOnPlayer(damage);

            lastAttackTime = Time.time; // Atualiza o tempo do último ataque
        }
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
            Gizmos.DrawLine(patrolPointA.position, patrolPointB.position);
        }

        // Desenha o alcance de ataque no editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
