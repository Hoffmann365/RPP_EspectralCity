using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantedSpirit_Shot : MonoBehaviour
{
        public float lifeTime = 3f;  // Tempo de vida do projétil

        void Start()
        {
            // Destruir o projétil após um determinado tempo
            Destroy(gameObject, lifeTime);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Destruir o projétil ao colidir com algo
            Destroy(gameObject);
        }
}
