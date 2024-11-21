using UnityEngine;

public class DoorProximity : MonoBehaviour
{
    public bool isPlayerNear = false; // Informação se o jogador está perto da porta

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true; // Jogador entrou na área de proximidade
            Debug.Log("Jogador entrou na proximidade da porta.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false; // Jogador saiu da área de proximidade
            Debug.Log("Jogador saiu da proximidade da porta.");
        }
    }
}