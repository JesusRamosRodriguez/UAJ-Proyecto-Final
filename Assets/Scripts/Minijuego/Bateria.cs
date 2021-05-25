using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bateria : MonoBehaviour {

    public float tiempo; // tiempo de desactivación
    Renderer rend;
    void Start ()
    {
        rend = GetComponent<Renderer>();
        Invoke("Desactiva", 0f);
    }
	
	void Activa()
    {
        rend.enabled = true;
        Invoke("Desactiva", tiempo);
    }

    void Desactiva()
    {
        rend.enabled = false;
        Invoke("Activa", tiempo);
    }
}
