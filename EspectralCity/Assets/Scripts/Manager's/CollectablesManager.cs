using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    public int qtdMoedas;
    public int qtdPontos;
    public int qtdKeys;

    private void OnEnable()
    {
        CollectablesObserver.AddKeysEvent += AddKeys;
        CollectablesObserver.RemoveKeysEvent += RemoveKeys;
        
        CollectablesObserver.AddCoinEvent += AddCoin;
        CollectablesObserver.RemoveCoinEvent += RemoveCoin;
    }

    private void OnDisable()
    {
        CollectablesObserver.AddKeysEvent -= AddKeys;
        CollectablesObserver.RemoveKeysEvent -= RemoveKeys;
        
        CollectablesObserver.AddCoinEvent -= AddCoin;
        CollectablesObserver.RemoveCoinEvent -= RemoveCoin;
    }

    void AddKeys()
    {
        qtdKeys += 1;
    }

    void AddCoin()
    {
        qtdMoedas += 1;
    }

    void RemoveKeys()
    {
        qtdKeys -= 1;
    }

    void RemoveCoin()
    {
        qtdMoedas -= 1;
    }

    public int GetKeys()
    {
        return qtdKeys; // Retorna a quantidade de chaves
    }

    public int GetMoedas()
    {
        return qtdMoedas;
    }
    
}
