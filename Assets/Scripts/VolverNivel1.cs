using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolverNivel1 : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameManager.instance.Nivel1();
        }

    }
}
