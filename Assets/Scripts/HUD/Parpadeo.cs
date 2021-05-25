using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parpadeo : MonoBehaviour {
    public float Rate;
    bool visible = true;

	void Start () {
        InvokeRepeating("Parp", Rate, Rate);
	}

    void Parp()
    {
        if (visible)
        {
            visible = false;
            this.gameObject.SetActive(false);
        }
        else
        {
            visible = true;
            this.gameObject.SetActive(true);
        }
    }
}
