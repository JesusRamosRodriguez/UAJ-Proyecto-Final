using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : MonoBehaviour {

    bool puedoDisparar = true;

    //audio
    AudioSource fuenteAudio;
    public AudioClip sprayAudio;


    GameObject vision, spray;
     void Start()
    {
        fuenteAudio = GetComponent<AudioSource>();
        spray = transform.GetChild(4).gameObject;
        vision = transform.GetChild(0).gameObject;
    }
    void Update() {

        //Si esta dentro de la vision y ha pasado el coldown puede disparar
        if (vision.GetComponent<Detect>().LeVeo() && puedoDisparar)
        {
            puedoDisparar = false;
            Dispara();

        }

    }

    void Dispara()
    {
        gameObject.GetComponent<Patrol>().Spray();
        Invoke("EfectuaDisparo", 0.5f);//Tiene que tener el mismo tiempo de invocacion que el metedo spray de patrol
    }

    void EfectuaDisparo()
    {
        if (vision.GetComponent<Detect>().LeVeo())
        {
            GameObject.FindWithTag("Player").GetComponent<Controller>().Spray();
        }
        //audio
        fuenteAudio.clip = sprayAudio;
        fuenteAudio.Play();

        spray.SetActive(true);
        Invoke("Desactiva", 1f);
        Invoke("PuedesDisparar", 4);//4 segundos para que pueda volver a disparar
    }

    void PuedesDisparar()
    {
        puedoDisparar = true;
    }
    void Desactiva()
    {
        spray.SetActive(false);
    }
}
