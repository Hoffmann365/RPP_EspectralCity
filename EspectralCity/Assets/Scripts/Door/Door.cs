using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen; // Verifica se a porta está aberta
    
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;
    
    private DoorProximity doorProximity; // Referência ao script do objeto filho
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        
        // Acessa o script DoorProximity do objeto filho
        doorProximity = GetComponentInChildren<DoorProximity>();
    }

    // Update is called once per frame
    void Update()
    {
        // Verifica se o jogador está perto da porta e pressionou "F"
        if (doorProximity != null && doorProximity.isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            if (!isOpen)
            {
                OpenDoor();
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
}
