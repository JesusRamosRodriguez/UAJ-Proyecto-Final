using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mecanica : MonoBehaviour {
    //audio
    AudioSource fotoSonido, famosoSonido, failSonido;

    public float tiempoFoto;        //retardo entre foto y foto
    public int fotos = 0;           //variable que guarda las fotos "buenas"

    bool puedeFoto = true;          //variable aux para permitir hacer foto
    int colisiones = 0;
    bool dentro;
    int puntuacion = 0;             //variable que guarda la puntuación
    int multiplicador = 2000;        //multiplicador de la puntuaicón
    bool fin = false;

    public GameObject clickIcon, continueText,    //Guarda el icono de click/continuar
                        polaroid, //Objeto animado polaroid
                        finText; //Texto final
    bool fotoHecha = false; //True si ya ha hecho una foto bien

    private void Start()
    {
        //sonido
        AudioSource[] audios = GetComponents<AudioSource>();
        fotoSonido = audios[0];
        famosoSonido = audios[1];
        failSonido = audios[2];
    }

    void OnTriggerEnter2D (Collider2D other)
	{
        if (other.tag == "Objetivo")
        {
            colisiones++;
            dentro = true;
		}
	}

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Objetivo")
        {
            colisiones--;
            dentro = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Objetivo")
        {
            
            dentro = true;
        }
    }

    void Update()
    {
		if(fotoHecha && Input.GetKeyDown(KeyCode.Space) && GameManager.instance.Carretes() != 0)    //Termina el juego si ya ha hecho una foto y pulsa Espacio
        {
            fin = true;
            continueText.SetActive(false);
            finText.SetActive(true);
            Time.timeScale = 0.5f;
            Invoke("Finalizar", 2f);
        }

        if (Input.GetMouseButtonDown (0) && puedeFoto && (GameManager.instance.Carretes () > 0 || GameManager.instance.carreteEspecial > 0) && !fin) {
			//audio
			fotoSonido.Play ();

			// El primer carrete que se gasta es el especial
			if (GameManager.instance.carreteEspecial == 1)
				GameManager.instance.carreteEspecial--;
			else
				GameManager.instance.carretes--;
            

			fotos = fotos + colisiones;
			puedeFoto = false;

            if (dentro)
            {
                famosoSonido.Play();
                GameManager.instance.SumaPuntos(multiplicador, "minijuego");
                polaroid.GetComponent<PolaroidAnim>().SetPlay(true);
                if (!fotoHecha) PrimeraFotoBien();  //Primera foto hecha
            }
            else
            {
                TelemetrySystem.Instance.singleEvent("FalloMinijuego", GameManager.instance.getLevelNumber());
                failSonido.Play();  //Fallo
            }

			//Puntuacion();
			Invoke ("PuedeFoto", tiempoFoto);    // Sólo puede echar una foto si han pasado n segundos
		} else if (GameManager.instance.Carretes () == 0 && GameManager.instance.carreteEspecial == 0 && fotoHecha) //Se le acaban los carretes, pero ha hecho una foto -> Volver a nivel 
        {
            fin = true;
            continueText.SetActive(false);
            finText.SetActive(true);
            Time.timeScale = 0.5f;
            Invoke("Finalizar", 2f);
		}
        else if (GameManager.instance.Carretes() == 0 && GameManager.instance.carreteEspecial == 0 && !fotoHecha)
        {
            Time.timeScale = 1f;
            GameManager.instance.Pierde();
        }
			
    }

    void PuedeFoto()
    {
        puedeFoto = true;
    }

    void Puntuacion()
    {
        puntuacion = puntuacion + multiplicador;
    }

    void PrimeraFotoBien()
    {
        fotoHecha = true;

        //Avisar al jugador de que ya puede continuar
        clickIcon.SetActive(false);
        continueText.SetActive(true);
    }

    public bool IsFotoHecha()
    {
        return fotoHecha;
    }

    void Finalizar()
    {
        
        GameManager.instance.Minijuego();
        GameManager.instance.FinMinijuego();
        Time.timeScale = 1f;

    }
}