using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObserver
{
    public static event Action<string> NextScene;
    public static event Action<int> DamageOnPlayer;
    public static event Action<int> DamageOnShadow; 

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
}
