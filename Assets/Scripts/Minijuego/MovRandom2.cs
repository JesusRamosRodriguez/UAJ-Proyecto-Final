using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovRandom2 : MonoBehaviour
{

    private Rigidbody2D rb2d;
    public float MovX; //test
    public float MovY; //test
    public float V;
    Animator animacion;

    void Start()
    {
        MovX = Random.Range(1, 3);
        MovY = Random.Range(1, 3);
        rb2d = GetComponent<Rigidbody2D>();
        animacion = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 mov = new Vector2(MovX, MovY);
        rb2d.velocity = (mov) * V;

        if (MovX < 0)
            animacion.SetTrigger("izquierda");
        else animacion.SetTrigger("derecha");

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "TopeV")
        {
            MovX = -MovX;
        }
        else if (other.tag == "TopeH")
        {
            MovY = -MovY;
        }

    }
}
