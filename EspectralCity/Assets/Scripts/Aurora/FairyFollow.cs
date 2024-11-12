using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyFollow : MonoBehaviour
{
    public Transform player;  // A referência para o jogador
    public Vector3 offset = new Vector3(1, 2, 0); // Offset da posição da fada em relação ao player
    public float smoothSpeed = 0.125f;  // Velocidade de suavização do movimento
    public float hoverAmplitude = 0.5f;  // Amplitude do movimento de flutuação
    public float hoverFrequency = 2f;  // Frequência do movimento de flutuação

    private Vector3 initialOffset;

    void Start()
    {
        // Configura o offset inicial (pode ser ajustado na Unity também)
        initialOffset = offset;
    }

    void Update()
    {
        FollowPlayer();
        HoverEffect();
    }

    void FollowPlayer()
    {
        // Calcula a posição alvo baseada na posição do player com um offset
        Vector3 targetPosition = player.position + offset;
        // Interpola suavemente a posição atual da fada para a posição alvo
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }

    void HoverEffect()
    {
        // Adiciona um efeito de flutuação (vai e vem) na posição Y
        float hover = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        transform.position += new Vector3(0, hover, 0) * Time.deltaTime;
    }
}
