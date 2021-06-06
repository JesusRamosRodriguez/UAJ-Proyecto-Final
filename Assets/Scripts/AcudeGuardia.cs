using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcudeGuardia : MonoBehaviour {

    public GameObject go;
    Detect camara;

    bool hasDetectedPlayer;

    private void Start()
    {
        hasDetectedPlayer = false;
        camara = transform.parent.transform.GetChild(0).gameObject.GetComponent < Detect>();
    }
    private void Update()
    {
        if (camara.LeVeo() == true)
        {
            if (!hasDetectedPlayer)
            {
                TelemetrySystem.Instance.singleEvent("CamaraDetectaJugador", GameManager.instance.getLevelNumber());
                hasDetectedPlayer = true;
            }
        }
        hasDetectedPlayer = false;
    }
}
