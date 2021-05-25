using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puertas1I : MonoBehaviour
{

    bool abierta = false;
    SpriteRenderer sprite;
    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Espalda") || collision.CompareTag("Frente") && !abierta)
        {
            sprite.transform.right = Vector2.right;
            abierta = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Espalda") || collision.CompareTag("Frente") && abierta)
        {
            sprite.transform.up = Vector2.left;
            abierta = false;
        }
    }
}
