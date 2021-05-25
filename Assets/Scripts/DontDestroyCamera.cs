using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyCamera : MonoBehaviour {

    public static DontDestroyCamera instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
}
