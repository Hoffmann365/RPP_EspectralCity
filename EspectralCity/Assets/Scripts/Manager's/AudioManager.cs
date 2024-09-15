using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip clipPulo, clipColetavel,clipHit, clipGameOver;

    private void OnEnable()
    {
        AudioObserver.PlaySfxEvent += TocarEfeitoSonoro;
        AudioObserver.PlayMusicEvent += TocarMusica;
        AudioObserver.StopMusicEvent += PararMusica;
        
    }

    private void OnDisable()
    {
        AudioObserver.PlaySfxEvent -= TocarEfeitoSonoro;
        AudioObserver.PlayMusicEvent -= TocarMusica;
        AudioObserver.StopMusicEvent -= PararMusica;
        
    }

    void TocarEfeitoSonoro(string nomeDoClip)
    {
        switch (nomeDoClip)
        {
            case "pulo":
                sfxSource.PlayOneShot(clipPulo);
                break;
            case "coletavel":
                sfxSource.PlayOneShot(clipColetavel);
                break;
            case "hit":
                sfxSource.PlayOneShot(clipHit);
                break;
            case "gameover":
                sfxSource.PlayOneShot(clipGameOver);
                break;
            default:
                Debug.LogError($"efeito sonoro {nomeDoClip} não encontrado");
                break;
            
        }
    }

    void TocarMusica()
    {
        musicSource.Play();
    }

    void PararMusica()
    {
        musicSource.Stop();
    }
    
}
