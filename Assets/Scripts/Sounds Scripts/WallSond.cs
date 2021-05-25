using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSond : MonoBehaviour {

    static AudioSource fuenteAudio;

    void Start () {
        fuenteAudio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            fuenteAudio.Play();
        }
    }
}
