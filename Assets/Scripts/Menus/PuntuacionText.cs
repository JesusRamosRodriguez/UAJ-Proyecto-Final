using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuntuacionText : MonoBehaviour {

	Text stringTexto;
    int indiceNivel;

	void Awake()
	{
		stringTexto = gameObject.GetComponent<Text>();
	}

	void Start()
	{
		SetText ();
	}

	void SetText () {
        stringTexto.text = GameManager.instance.TextoPuntuacion ();
	}
										

}
