using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arnold : MonoBehaviour {
    bool face = false;

    void Start()
	{
        InvokeRepeating("ShowFace", 1.0f, 1.0f);
    }

    public bool IsFace()
    {
        return face;
    }

    public void ShowFace()
    {
        face = !face;
    }
}
