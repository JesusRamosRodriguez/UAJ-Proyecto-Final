using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSource : MonoBehaviour {

    //clips de música accesibles desde el editor
    public AudioClip mainMusic1;


    AudioSource fuenteAudio;

	void Start () {
        fuenteAudio = GetComponent<AudioSource>();

        //musicas
        PlayMain();
	}

    public void PlayMain()
    {
        fuenteAudio.clip = mainMusic1;
        fuenteAudio.volume = 0.5f;
        fuenteAudio.Play();
    }


}
