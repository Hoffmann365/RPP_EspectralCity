using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    public string message;  // Mensagem a ser exibida
    public FairyGuide fairyGuide;  // Referência ao script da fada

    private bool hasTriggered = false;  // Para garantir que o trigger só seja ativado uma vez

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            fairyGuide.ShowHint(message);  // Exibe a mensagem usando o script da fada
            hasTriggered = true;  // Marca o trigger como ativado para não repetir
        }
    }
}