using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    //sonido
    AudioSource fuenteAudio;
    bool dentro; //si el jugardor esta cerca del boton(Collider), dentro true

    private void Start()
    {
        //sonido
        fuenteAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) MuerteLaser();
    }

    void MuerteLaser()
    {
        TelemetrySystem.Instance.singleEvent("Muerte", GameManager.instance.getLevelNumber());
        fuenteAudio.Play();
        GameManager.instance.Pierde();
    }
}
