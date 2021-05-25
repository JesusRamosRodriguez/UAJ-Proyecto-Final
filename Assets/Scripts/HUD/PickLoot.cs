using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickLoot : MonoBehaviour
{

    Text stringTexto;

    void Start()
    {
        stringTexto = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        stringTexto.text = GameManager.instance.Loot().ToString() + " x";
    }
}
