using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioObserver
{
    public static event Action PlayMusicEvent;
    
    public static event Action StopMusicEvent;

    public static void OnPlayMusicEvent()
    {
        PlayMusicEvent?.Invoke();
    }

    public static void OnStopMusicEvent()
    {
        StopMusicEvent?.Invoke();
    }
}
