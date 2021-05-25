using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autodestruccion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("Destruir", 0.1f);
	}

    void Destruir()
    {
        Destroy(this.gameObject);
    }
}
