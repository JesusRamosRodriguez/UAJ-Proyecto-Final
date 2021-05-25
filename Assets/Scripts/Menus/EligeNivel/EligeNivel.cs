using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EligeNivel : MonoBehaviour {



    public void Activa()
    {
        GameObject nivel1 = gameObject.transform.GetChild(0).gameObject,
            nivel2 = gameObject.transform.GetChild(1).gameObject,
            nivel3 = gameObject.transform.GetChild(2).gameObject;
        Text stringTexto = nivel1.transform.GetChild(1).gameObject.GetComponent<Text>(),
            stringTexto2 = nivel2.transform.GetChild(1).gameObject.GetComponent<Text>(),
            stringTexto3 = nivel3.transform.GetChild(1).gameObject.GetComponent<Text>();

        gameObject.SetActive(true);
        string posible = GameManager.instance.EligeNivel();
        if (posible.Length == 3)
        {
            nivel1.transform.GetChild(1).GetComponent<Text>().text = "Puntuacion: " + GameManager.instance.PuntuacionMaxima(1);
            nivel2.SetActive(true);
            stringTexto2.text = "Puntuacion: " + GameManager.instance.PuntuacionMaxima(2);
            nivel3.SetActive(true);
            stringTexto3.text = "Puntuacion: " + GameManager.instance.PuntuacionMaxima(3);
        }
        else if(posible.Length == 2)
        {
            stringTexto.text = "Puntuacion: " + GameManager.instance.PuntuacionMaxima(1);
            nivel2.SetActive(true);
            stringTexto2.text = "Puntuacion: " + GameManager.instance.PuntuacionMaxima(2);
            nivel3.SetActive(false);
        }
        else
        {

            stringTexto.text = "Puntuacion: " + GameManager.instance.PuntuacionMaxima(1);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }

    }
    

    public void Desactiva()
    {
        gameObject.SetActive(false);
    }

    public void NivelElige1()
    {
        GameManager.instance.Nivel1();
    }
    public void NivelElige2()
    {
        GameManager.instance.Nivel2();
    }
    public void NivelElige3()
    {
        GameManager.instance.Nivel3();
    }
}
