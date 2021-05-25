using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transicion : MonoBehaviour {
	void Start () {
        GameObject tr = GameObject.FindGameObjectWithTag("Transicion");
        tr.transform.GetChild(0).gameObject.SetActive(true);
    }
}
