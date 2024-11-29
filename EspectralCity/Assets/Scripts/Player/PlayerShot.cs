using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    private Rigidbody2D rig;

    public float speed;
    
    public bool isRight;

    public int bulletDmg;

    private CogumeloPatrol cog;
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ShadowAI shadow = other.gameObject.GetComponent<ShadowAI>();
        CogumeloPatrol cog = other.gameObject.GetComponent<CogumeloPatrol>();
        SkeletonEnemy skeleton = other.gameObject.GetComponent<SkeletonEnemy>();
        CogumeloBoss pcog = other.gameObject.GetComponent<CogumeloBoss>();
        if (shadow != null)
        {
            shadow.Damage(bulletDmg);
            Destroy(gameObject);
        }
        if (cog != null)
        {
            cog.Damage(bulletDmg);
            Destroy(gameObject);
        }

        if (skeleton != null)
        {
            skeleton.Damage(bulletDmg);
            Destroy(gameObject);
        }

        if (pcog != null)
        {
            pcog.Damage(bulletDmg);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRight)
        {
            rig.velocity = Vector2.right * speed;
        }
        else
        {
            rig.velocity = Vector2.left * speed;
        }
    }
}
