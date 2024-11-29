using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public bool isOpen; // Verifica se a porta está aberta
    private bool isPlayerNear; // Verifica se o jogador está perto da porta
    public bool isBossDead = false; // Verifica se o boss foi morto

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;

    // Referência ao script do boss
    public GameObject boss; // Referência ao objeto Boss

    // Mensagem que será exibida ao jogador
    public GameObject lockedMessage; // Referência a um GameObject UI para mostrar a mensagem de "Aguarde o boss ser derrotado"

    private Player player;
    private DoorProximity doorProximity; // Referência ao script do objeto filho para detectar proximidade

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // Inicializa a porta como fechada e invisível
        spriteRenderer.enabled = false;
        coll.enabled = false;

        // Certifique-se de que a mensagem de bloqueio esteja desativada no início
        if (lockedMessage != null)
        {
            lockedMessage.SetActive(false);
        }

        // Acessa o script DoorProximity do objeto filho
        doorProximity = GetComponentInChildren<DoorProximity>();
    }

    void Update()
    {
        // Verifica se o boss foi derrotado
        if (boss == null)
        {
            isBossDead = true; // O boss foi derrotado
        }

        // Verifica se a luta contra o boss foi iniciada
        if (boss != null && boss.GetComponent<CogumeloBoss>().fightStarted) 
        {
            // Ativa a porta quando a luta começar
            if (!spriteRenderer.enabled)
            {
                spriteRenderer.enabled = true;
                coll.enabled = true;
            }
        }

        // Se o jogador estiver perto da porta e pressionar "F"
        if (doorProximity != null && doorProximity.isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            if (isBossDead && !isOpen)
            {
                OpenDoor();
            }
            else if (!isBossDead)
            {
                ShowLockedMessage(); // Mostra a mensagem se o boss não foi derrotado
            }
        }
    }

    private void OpenDoor()
    {
        isOpen = true; // Marca que a porta foi aberta
        spriteRenderer.enabled = false; // Desativa o sprite da porta
        coll.isTrigger = true; // Torna o colisor um trigger para permitir passagem
        Debug.Log("Porta aberta!"); // Mensagem para depuração
    }

    private void ShowLockedMessage()
    {
        // Exibe a mensagem temporariamente
        if (lockedMessage != null)
        {
            lockedMessage.SetActive(true);
            Debug.Log("Porta trancada, aguarde a derrota do boss!");

            // Opcional: Desativar a mensagem após um tempo
            Invoke(nameof(HideLockedMessage), 2f);
        }
    }

    private void HideLockedMessage()
    {
        if (lockedMessage != null)
        {
            lockedMessage.SetActive(false);
        }
    }
}
