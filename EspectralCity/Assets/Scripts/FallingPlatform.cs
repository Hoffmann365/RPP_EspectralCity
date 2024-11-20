using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D collider2D;

    public float fallDelay = 2f; // Tempo para a plataforma cair
    public float respawnDelay = 3f; // Tempo para reaparecer
    Vector2 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<CapsuleCollider2D>();
        initialPosition = transform.position; // Salva a posição inicial
    }

    IEnumerator FallAndRespawn()
    {
        // Aguarda antes de fazer a plataforma cair
        yield return new WaitForSeconds(0.2f);
        rb.bodyType = RigidbodyType2D.Dynamic; // Torna a plataforma dinâmica para cair

        // Aguarda o tempo de queda
        yield return new WaitForSeconds(fallDelay);

        // "Desaparece" a plataforma
        rb.bodyType = RigidbodyType2D.Static; // Torna estático novamente
        rb.Sleep(); // Coloca o Rigidbody em repouso
        spriteRenderer.enabled = false; // Desativa o sprite
        collider2D.isTrigger = true; // Define o colisor como trigger

        // Aguarda o tempo de reaparecimento
        yield return new WaitForSeconds(respawnDelay);

        // Reaparece a plataforma
        transform.position = initialPosition; // Reseta a posição inicial
        rb.WakeUp(); // Reativa o Rigidbody
        spriteRenderer.enabled = true; // Reativa o sprite
        collider2D.isTrigger = false; // Restaura o colisor
    }

    // Detecta a colisão com o player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FallAndRespawn());
        }
    }
}