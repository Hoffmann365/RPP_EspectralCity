  using System;
  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
  public GameObject prefabFumacinha;

  private void OnEnable()
  {
    ParticleObserver.ParticleSpawnEvent += SpawnarParticulas;
  }

  private void OnDisable()
  {
    ParticleObserver.ParticleSpawnEvent -= SpawnarParticulas;
  }

  public void SpawnarParticulas(Vector3 posicao)
  {
    Instantiate(prefabFumacinha, posicao, Quaternion.identity);
  }
}
