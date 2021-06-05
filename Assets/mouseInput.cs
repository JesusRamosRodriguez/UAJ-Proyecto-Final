using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            TelemetrySystem.Instance.singleEvent("ClickCinematica", 0);
        }

    }
}
