using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CuentaAtras : MonoBehaviour {

    public Text crono;
    public float tiempo = 0, limitTiempo = 45f;
    public float tiempoGuardado; //El tiempo guardado al iniciar el minijuego

    private float starTime;
    private bool fin = false;

    public GameObject objetivo;

    void Start () {
        starTime = Time.time;

        if (!GameManager.instance.MinijuegoTerminado())
            tiempoGuardado = 0;
	}
	
	void Update () {

        tiempo = Time.time - starTime + tiempoGuardado;

        int seg = (int)(tiempo % 60);
        // si el tiempo llega a 60 (cuenta atrás a 0) y ha hecho una foto, vuelve a nivel
        if (seg > limitTiempo && objetivo.GetComponent<Mecanica>().IsFotoHecha())
            GameManager.instance.FinMinijuego();
        else if (seg > limitTiempo && !objetivo.GetComponent<Mecanica>().IsFotoHecha())
        {
            GameManager.instance.Pierde();
        }
        else
        {

            seg = (int)limitTiempo - seg;

            //formato texto
            string segundos = seg.ToString("0");

            if (int.Parse(segundos) < 10)
                segundos = "0" + segundos;
            crono.text = "0:" + segundos;
        }
	}

    public float FinPartida()
    {
        fin = true;
        crono.color = Color.yellow;
        return tiempo;
    }

    public float TiempoAntes()
    {
        return tiempo;
    }
    public void CambiaTiempo(float nuevo)
    {
        tiempoGuardado = nuevo;
    }
}
