using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            TelemetrySystem.Instance.singleEvent("ClickCinematica", 0);

        }

    }
}
