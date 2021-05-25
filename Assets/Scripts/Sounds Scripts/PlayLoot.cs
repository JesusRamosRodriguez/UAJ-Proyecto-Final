using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLoot : MonoBehaviour {

    static AudioSource fuenteAudio;

    void Start () {
        fuenteAudio = GetComponent<AudioSource>();
    }
	
    public static void PlaySound()
    {
        fuenteAudio.Play();
    }
}
