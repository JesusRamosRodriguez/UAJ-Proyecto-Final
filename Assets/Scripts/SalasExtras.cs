using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalasExtras : MonoBehaviour {

    public float x, y;



    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.transform.position = new Vector3(x, y, 0);
        }
    }
}
