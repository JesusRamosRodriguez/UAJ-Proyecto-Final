using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnInteract : MonoBehaviour {
    //sonido
    AudioSource fuenteAudio;
    bool dentro; //si el jugardor esta cerca del boton(Collider), dentro true

    private void Start()
    {
        //sonido
        fuenteAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Si esta en el boton y se pulsa la E 
        if (dentro && GameObject.FindWithTag("Player").GetComponent<Controller>().CompruebaE())
        {
            //sonido
            fuenteAudio.Play();
        }

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
