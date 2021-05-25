using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed;
    Rigidbody2D rb2D;
    public float time = 2;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        InvokeRepeating("ChangeSpeed", time, time);
    }
    void FixedUpdate()
    {
        rb2D.MoveRotation(rb2D.rotation + speed * Time.fixedDeltaTime);

    }

    void ChangeSpeed()
    {
        speed = -speed;
    }
}
