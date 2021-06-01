using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcudeGuardia : MonoBehaviour {

    public GameObject go;
    Detect camara;

    private void Start()
    {
        camara = transform.parent.transform.GetChild(0).gameObject.GetComponent < Detect>();
    }
    private void Update()
    {
        if (camara.LeVeo() == true)
        {
            Debug.Log("llama guardia");
            TelemetrySystem.Instance.singleEvent("CamaraDetectaJugador", GameManager.instance.getLevelNumber());
        }
    }
}
