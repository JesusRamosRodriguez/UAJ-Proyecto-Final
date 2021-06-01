using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Cronometro : MonoBehaviour {

    public Text crono;
    public float tiempo = 0;
    public float tiempoGuardado; //El tiempo guardado al iniciar el minijuego

    private float starTime;
    private bool fin = false;

    void Start () {
        starTime = Time.time;

        if (!GameManager.instance.MinijuegoTerminado())
            tiempoGuardado = 0;
	}
	
	void Update () {

        tiempo = Time.time - starTime + tiempoGuardado;

        int min = ((int)tiempo / 60);
        int seg = (int)(tiempo % 60);

        string minutos = min.ToString();
        string segundos = seg.ToString("0");

        if (int.Parse(segundos) < 10)
            segundos = "0" + segundos;
        crono.text = minutos + ":" + segundos;
	}

    public float FinPartida()
    {
        TelemetrySystem.Instance.valueEvent("TiempoFinalNivel ", (int)tiempo, GameManager.instance.getLevelNumber());
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

    public float devuelveTiempoGuardado ()
    {
        return tiempoGuardado;
    }
}
