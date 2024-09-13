using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObserver
{
    public static event Action<string> NextScene;

    public static void OnNextScene(string obj)
    {
        NextScene?.Invoke(obj);
    }
}
