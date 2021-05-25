using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carretes : MonoBehaviour {

    Text stringTexto;

	void Start () {
        stringTexto = gameObject.GetComponent<Text>();
    }
	
	void Update () {
        stringTexto.text = "x " + GameManager.instance.Carretes().ToString();
	}
}
