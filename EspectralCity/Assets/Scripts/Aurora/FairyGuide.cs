using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FairyGuide : MonoBehaviour
{
    public GameObject textGameObject;  // GameObject que contém o texto
    public Text messageBox;  // Caixa de texto para exibir a mensagem da fada
    public float typingSpeed = 0.05f;  // Velocidade da digitação
    public AudioSource typingSound;  // Som de digitação

    private Coroutine messageCoroutine;

    // Chama essa função para exibir uma dica
    public void ShowHint(string message)
    {
        if (messageCoroutine != null) StopCoroutine(messageCoroutine);
        messageCoroutine = StartCoroutine(DisplayMessage(message));
    }

    // Exibe a mensagem com efeito de digitação
    IEnumerator DisplayMessage(string message)
    {
        textGameObject.SetActive(true);  // Ativa o GameObject contendo o texto
        messageBox.text = "";  // Limpa o texto atual
        messageBox.gameObject.SetActive(true);  // Ativa o componente Text

        foreach (char letter in message.ToCharArray())
        {
            messageBox.text += letter;

            // Toca o som de digitação em cada letra (ou a cada 2-3 letras para suavizar)
            if (typingSound != null && letter != ' ')
            {
                typingSound.Play();
            }

            yield return new WaitForSeconds(typingSpeed);  // Espera antes de exibir a próxima letra
        }

        // Após terminar de exibir o texto, espera um tempo e desativa o GameObject
        yield return new WaitForSeconds(1f);  // Tempo para exibir a mensagem após a digitação

        textGameObject.SetActive(false);  // Desativa o GameObject contendo o texto
    }
}