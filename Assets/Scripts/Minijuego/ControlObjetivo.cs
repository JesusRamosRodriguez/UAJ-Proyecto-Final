using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlObjetivo : MonoBehaviour {

    private Rigidbody2D rb2d;
    public float MovX;
    public float MovY;
    public float V;

    void Start() {
        MovX = Random.Range(1, 3);
        MovY = Random.Range(1, 3);
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        Vector2 mov = new Vector2(MovX, MovY);
        Vector2 rat = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		rb2d.velocity = (rat+ mov)*V;
    }

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "TopeH") {
			MovY = -MovY;
		} 

		else if (other.gameObject.tag == "TopeV") {
			MovX = -MovX;
		}
			
	}
}
