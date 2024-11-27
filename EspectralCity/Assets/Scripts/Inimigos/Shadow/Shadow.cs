using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [Header("Variáveis: ")] 
    public float speed;
    public float walktime;
    public bool walkRight = true;
    private float timer;
    private bool isAtk = false;
    

    [Header("Componentes: ")] 
    private Rigidbody2D rig;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        timer += Time.deltaTime;

        if (timer >= walktime)
        {
            walkRight = !walkRight;
            timer = 0f;
        }

        if (walkRight)
        {
            if (isAtk == false)
            {
                anim.SetInteger("transition", 1);
            }

            transform.eulerAngles = new Vector2(0, 0);
            rig.velocity = Vector2.right * speed;
        }

        if (!walkRight)
        {
            if (isAtk == false)
            {
                anim.SetInteger("transition", 1);
            }

            transform.eulerAngles = new Vector2(0, 180);
            rig.velocity = Vector2.left * speed;
        }
    }
}
