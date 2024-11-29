using System.Collections;
using UnityEngine;

public class CogumeloBoss : MonoBehaviour
{
    public float speed = 3f; // Velocidade de movimento do boss
    public float chargeSpeed = 8f; // Velocidade da investida
    public float attackRange = 4f; // Alcance para realizar a investida
    public float chargeDuration = 1f; // Duração da investida
    public float restDuration = 2f; // Tempo parado após a investida
    public int health = 100; // Vida do boss
    public int cogumeloDmg = 20; // Dano que o boss causa ao player
    public bool fightStarted = false;
    private bool actived = false;

    private bool isCharging = false;
    private bool isResting = false;
    private bool isPlayerInRange = false; // Estado atualizado pelo objeto filho
    private Transform player;
    private Rigidbody2D rig;
    private Animator anim;
    private float groundY; // Guarda a posição fixa no eixo Y (chão)
    
    private float damageCooldown = 1f; // Tempo em segundos entre os tics de dano
    private float lastDamageTime = -999f; // Tempo em que o último dano foi causado

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Procura o jogador

        // Grava a posição inicial no eixo Y
        groundY = transform.position.y;

        // Trava o Rigidbody no eixo Y
        rig.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    private void Update()
    {
        if (health <= 0 || !isPlayerInRange) return;

        if (!actived)
        {
            StartFight();
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isCharging && !isResting)
        {
            if (distanceToPlayer > attackRange)
            {
                FollowPlayer(); // Segue o player
            }
            else
            {
                StartCoroutine(Charge()); // Faz a investida
            }
        }
    }

    private void FollowPlayer()
    {
        anim.SetInteger("transition", 1); // Animação de andar

        // Calcula a direção do movimento (em relação ao player)
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // Movimenta apenas no eixo X e mantém a posição Y constante
        transform.position = new Vector3(
            Mathf.MoveTowards(transform.position.x, transform.position.x + direction, speed * Time.deltaTime),
            groundY,
            transform.position.z
        );

        // Faz o inimigo olhar na direção em que está andando
        Flip(direction);
    }

    private IEnumerator Charge()
    {
        isCharging = true;
        anim.SetTrigger("charge"); // Animação de investida
        Vector2 chargeDirection = (player.position - transform.position).normalized;

        float elapsedTime = 0f;

        while (elapsedTime < chargeDuration)
        {
            Vector2 newPosition = (Vector2)transform.position + chargeDirection * chargeSpeed * Time.deltaTime;

            // Verificar colisão antes de mover
            RaycastHit2D hit = Physics2D.Raycast(transform.position, chargeDirection, chargeSpeed * Time.deltaTime, LayerMask.GetMask("Wall"));
        
            if (hit.collider != null)
            {
                // Se detectar uma colisão, interrompa a investida
                break;
            }

            // Use MovePosition para mover com o Rigidbody2D
            rig.MovePosition(newPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isCharging = false;
        StartCoroutine(Rest());
    }


    private IEnumerator Rest()
    {
        isResting = true;
        anim.SetInteger("transition", 0); // Animação de parado
        yield return new WaitForSeconds(restDuration);
        isResting = false;
    }

    private void Flip(float direction)
    {
        // Apenas inverte se estiver se movendo para uma direção oposta ao que está olhando
        if ((direction > 0 && transform.localScale.x < 0) || (direction < 0 && transform.localScale.x > 0))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    public void StartFight()
    {
        fightStarted = true;
        actived = true;
        
        // Lógica para iniciar a luta (exemplo: invocar inimigos, tocar música de batalha, etc.)
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        anim.SetTrigger("hit"); // Animação de tomar dano

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //anim.SetTrigger("die"); // Animação de morte
        Destroy(gameObject, 1f); // Destroi o boss após a animação de morte
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isCharging)
            {
                // Verifica se o cooldown já passou
                if (Time.time >= lastDamageTime + damageCooldown)
                {
                    GameObserver.OnDamageOnPlayer(cogumeloDmg); // Causa dano ao player
                    lastDamageTime = Time.time; // Atualiza o tempo do último dano
                }
            }
        }
    }

    public void SetPlayerInRange(bool inRange)
    {
        isPlayerInRange = inRange; // Atualiza o estado do player na área
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Alcance de ataque
    }
}
