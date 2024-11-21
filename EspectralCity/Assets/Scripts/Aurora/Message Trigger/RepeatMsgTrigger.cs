using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatMsgTrigger : MonoBehaviour
{
    public string message;  // Mensagem a ser exibida
    public FairyGuide fairyGuide;  // Referência ao script da fada
    public float repeatDelay = 2f;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ShowMessage());// Exibe a mensagem usando o script da fada
        }
    }

    IEnumerator ShowMessage()
    {
        fairyGuide.ShowHint(message);
        yield return new WaitForSeconds(repeatDelay);
    }
}
