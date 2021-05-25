using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton : MonoBehaviour {
    //sonido
    AudioSource fuenteAudio;
    public GameObject camara;
	bool dentro, activado; //si el jugardor esta cerca del boton(Collider), dentro true
	//Activado controla si el boton esta activo o no

    private void Start()
    {
        //sonido
        fuenteAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
		//Si esta en el boton y se pulsa la E 
		if (dentro && GameObject.FindWithTag ("Player").GetComponent<Controller> ().CompruebaE ()) 
		{
			if (!activado) //si el boton esta activado llama a desactiva
            {
                //sonido
                fuenteAudio.Play();
                DesactivaBoton();
				activado = true;
			}
			else if (activado) // si el boton esta desactivado llama a activa
			{
				ActivaBoton();
				activado = false;
				//sonido
				fuenteAudio.Play();
			}
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

    void DesactivaBoton()
    {
        camara.transform.GetChild(0).gameObject.SetActive(false);
        if (this.gameObject.CompareTag("BotonLaser")) Destroy(this.gameObject);
    }
    void ActivaBoton()
    {
        camara.transform.GetChild(0).gameObject.SetActive(true);
    }
}
