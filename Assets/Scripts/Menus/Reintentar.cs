using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reintentar : MonoBehaviour {

    public void Reintenta()
    {
        GameManager.instance.Reintentar();
    }
}
