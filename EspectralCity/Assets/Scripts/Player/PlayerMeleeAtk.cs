using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAtk : MonoBehaviour
{
    private BoxCollider2D colliderAtkPLayer;

    public int MeleeAtkDmg;
    // Start is called before the first frame update
    void Start()
    {
        colliderAtkPLayer = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shadow"))
        {
            GameObserver.OnDamageOnShadow(MeleeAtkDmg);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.movement < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (Player.movement > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
