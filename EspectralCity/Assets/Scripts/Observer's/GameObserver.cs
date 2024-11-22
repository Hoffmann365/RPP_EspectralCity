using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObserver
{
    public static event Action<string> NextScene;
    public static event Action<int> DamageOnPlayer;
    public static event Action<int> DamageOnShadow;
    
    public static event Action<int> DamageOnCogumelo;
    
    public static event Action GameOver;

    public static void OnNextScene(string obj)
    {
        NextScene?.Invoke(obj);
    }

    public static void OnDamageOnPlayer(int obj)
    {
        DamageOnPlayer?.Invoke(obj);
    }

    public static void OnDamageOnShadow(int obj)
    {
        DamageOnShadow?.Invoke(obj);
    }

    public static void OnGameOver()
    {
        GameOver?.Invoke();
    }

    public static void OnDamageOnCogumelo(int obj)
    {
        DamageOnCogumelo?.Invoke(obj);
    }
}
