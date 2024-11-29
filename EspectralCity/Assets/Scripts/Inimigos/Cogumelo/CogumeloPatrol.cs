using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogumeloPatrol : MonoBehaviour
{
    public float speed = 2f;        // Velocidade de movimento do inimigo
    public float patrolDistance = 5f; // Distância total da patrulha
    public int health;
    public int cogumeloDmg;
    
    public bool alive = true;

    private Vector3 pointA; // Ponto inicial da patrulha
    private Vector3 pointB; // Ponto final da patrulha
    private Vector3 targetPosition; // Posição para onde o inimigo está indo
    
    private Rigidbody2D rig;
    private Animator anim;
    private BoxCollider2D coll;
    
    private Transform player;
    
    private void OnEnable()
    {
        GameObserver.DamageOnCogumelo += Damage;
    }

    private void OnDisable()
    {
        GameObserver.DamageOnCogumelo -= Damage;
    }

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Define os pontos de patrulha baseados na posição inicial
        pointA = transform.position - Vector3.right * (patrolDistance / 2);
        pointB = transform.position + Vector3.right * (patrolDistance / 2);
        targetPosition = pointA; // Começa indo para o ponto A
    }

    private void Update()
    {
        if (alive)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        anim.SetInteger("transition", 1);
        // Move o inimigo na direção do alvo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        
        float direction = targetPosition.x - transform.position.x;

        // Verifica se chegou ao alvo
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Alterna o alvo entre pointA e pointB
            targetPosition = targetPosition == pointA ? pointB : pointA;

            // Inverte o inimigo para olhar na direção correta
            Flip(direction);
        }
    }

    private void Flip(float direction)
    {
        // Ajusta o lado do inimigo com base na direção (1 para direita, -1 para esquerda)
        if (direction > 0 && transform.localScale.x < 0 || direction < 0 && transform.localScale.x > 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1; // Inverte o eixo X
            transform.localScale = localScale;
        }
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        anim.SetTrigger("hit");
        //som de hit
        
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
        Destroy(gameObject, 0.8f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObserver.OnDamageOnPlayer(cogumeloDmg);
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
         
        }
    }

    // Exibe as setas de patrulha no editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

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
