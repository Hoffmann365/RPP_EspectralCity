using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkRange : MonoBehaviour
{
    public bool playerInRange = false; // Informação se o jogador está perto da porta
    private SkeletonEnemy skeletonEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true; // Jogador entrou na área de proximidade
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false; // Jogador saiu da área de proximidade
        }
    }

    private void Start()
    {
        skeletonEnemy = GetComponentInParent<SkeletonEnemy>();
    }
    
}
