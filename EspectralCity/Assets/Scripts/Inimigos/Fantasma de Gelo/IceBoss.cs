using System.Collections;
using UnityEngine;

public class IceBoss : MonoBehaviour
{
    public float speed = 2f; // Velocidade de movimento do boss
    public float chargeSpeed = 6f; // Velocidade da investida
    public float attackRange = 5f; // Alcance para realizar a investida
    public float chargeDuration = 1.2f; // Duração da investida
    public float restDuration = 2.5f; // Tempo parado após a investida
    public int health = 80; // Vida do boss
    public int iceDamage = 15; // Dano que o boss causa ao player durante a investida
    public bool fightStarted = false;

    private bool isCharging = false;
    private bool isResting = false;
    private bool isPlayerInRange = false; // Atualizado pelo detector
    private Transform player;
    private Rigidbody2D rig;
    private Animator anim;

    private float damageCooldown = 1f; // Tempo entre os tics de dano
    private float lastDamageTime = -999f; // Último tempo de dano causado

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Acha o jogador
    }

    private void Update()
    {
        if (health <= 0 || !isPlayerInRange) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isCharging && !isResting)
        {
            if (distanceToPlayer > attackRange)
            {
                FollowPlayer();
            }
            else
            {
                StartCoroutine(Charge());
            }
        }
    }

    private void FollowPlayer()
    {
        anim.SetInteger("transition", 1); // Animação de flutuar

        // Direção em relação ao jogador
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // Movimento no eixo X
        transform.position = new Vector3(
            Mathf.MoveTowards(transform.position.x, player.position.x, speed * Time.deltaTime),
            transform.position.y, // Mantém a posição Y
            transform.position.z
        );

        // Olha para a direção do movimento
        Flip(direction);
    }

    private IEnumerator Charge()
    {
        isCharging = true;
        anim.SetTrigger("charge"); // Animação de ataque

        Vector2 chargeDirection = (player.position - transform.position).normalized;
        float elapsedTime = 0f;

        while (elapsedTime < chargeDuration)
        {
            Vector2 newPosition = (Vector2)transform.position + chargeDirection * chargeSpeed * Time.deltaTime;

            // Verifica colisões antes de mover
            RaycastHit2D hit = Physics2D.Raycast(transform.position, chargeDirection, chargeSpeed * Time.deltaTime, LayerMask.GetMask("Wall"));

            if (hit.collider != null)
            {
                break; // Para a investida se colidir com algo
            }

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
        anim.SetInteger("transition", 1); // Animação de parado
        yield return new WaitForSeconds(restDuration);
        isResting = false;
    }

    private void Flip(float direction)
    {
        if ((direction > 0 && transform.localScale.x < 0) || (direction < 0 && transform.localScale.x > 0))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        anim.SetTrigger("hit"); // Animação de dano

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("die"); // Animação de morte
        Destroy(gameObject, 1f); // Destroi após animação
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isCharging)
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                GameObserver.OnDamageOnPlayer(iceDamage); // Aplica dano ao jogador
                lastDamageTime = Time.time;
            }
        }
    }

    public void SetPlayerInRange(bool inRange)
    {
        isPlayerInRange = inRange; // Define se o jogador está no alcance
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Mostra o alcance no editor
    }
}
