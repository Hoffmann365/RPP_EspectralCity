using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class CollectablesObserver
{
    public static event Action AddKeysEvent;
    public static event Action RemoveKeysEvent;
    
    public static event Action AddCoinEvent;
    public static event Action RemoveCoinEvent;

    public static void OnAddKeysEvent()
    {
        AddKeysEvent?.Invoke();
    }

    public static void OnRemoveKeysEvent()
    {
        RemoveKeysEvent?.Invoke();
    }

    public static void OnAddCoinEvent()
    {
        AddCoinEvent?.Invoke();
    }

    public static void OnRemoveCoinEvent()
    {
        RemoveCoinEvent?.Invoke();
    }
}
