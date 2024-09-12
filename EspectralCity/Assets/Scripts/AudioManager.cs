using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;

    private void OnEnable()
    {
        AudioObserver.PlayMusicEvent += TocarMusica;
        AudioObserver.StopMusicEvent += PararMusica;
    }

    private void OnDisable()
    {
        AudioObserver.PlayMusicEvent -= TocarMusica;
        AudioObserver.StopMusicEvent -= PararMusica;
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
