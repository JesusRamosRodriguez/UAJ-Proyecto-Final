using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bombillas : MonoBehaviour
{

    Text stringTexto;

    void Start()
    {
        stringTexto = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        stringTexto.text = "x " + GameManager.instance.Bombillas().ToString();
    }
}
