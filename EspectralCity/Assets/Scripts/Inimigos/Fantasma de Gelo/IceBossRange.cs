using System;
using UnityEngine;

public class IceBossRange : MonoBehaviour
{
    private IceBoss boss; // Referência ao script do boss (pai)

    private void Start()
    {
        boss = GetComponentInParent<IceBoss>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.SetPlayerInRange(true); // Notifica o boss que o player entrou no range
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.SetPlayerInRange(false); // Notifica o boss que o player saiu do range
        }
    }
}