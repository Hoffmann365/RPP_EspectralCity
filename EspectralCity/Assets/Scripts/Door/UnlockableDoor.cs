using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableDoor : MonoBehaviour
{
    public bool hasKey; // Verifica se o jogador tem a chave
    public bool isOpen; // Verifica se a porta está aberta
    private int qtdKeys;


    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;

    // Mensagem que será exibida ao jogador
    public GameObject lockedMessage; // Referência a um GameObject UI para mostrar a mensagem
    
    private DoorProximity doorProximity; // Referência ao script do objeto filho

    private Player player;
    
    public CollectablesManager collectables;

    // Start é chamado antes da primeira atualização
    void Start()
    {
        collectables = FindObjectOfType<CollectablesManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        
        // Acessa o script DoorProximity do objeto filho
        doorProximity = GetComponentInChildren<DoorProximity>();

        // Certifique-se de que a mensagem esteja desativada no início
        if (lockedMessage != null)
        {
            lockedMessage.SetActive(false);
        }
    }

    private void Update()
    {
        qtdKeys = collectables.GetKeys();
        hasKey = qtdKeys > 0;
        // Verifica se o jogador está perto da porta e pressionou "F"
        if (doorProximity != null && doorProximity.isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            if (hasKey && !isOpen)
            {
                OpenDoor();
            }
            else if (!hasKey)
            {
                ShowLockedMessage();
            }
        }
    }
    

    private void OpenDoor()
    {
        CollectablesObserver.OnRemoveKeysEvent();
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
            Debug.Log("Porta trancada, é necessário uma chave para abrir");

            // Opcional: Desativar a mensagem após um tempo
            Invoke(nameof(HideLockedMessage), 1.8f);
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
