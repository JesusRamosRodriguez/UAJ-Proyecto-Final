using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscuridad : MonoBehaviour {


    bool dentro= false;
    SpriteRenderer spriteRen;
    private void Start()
    {
        spriteRen = gameObject.GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        spriteRen.enabled = !dentro;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            dentro = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            dentro = false;
    }
}
