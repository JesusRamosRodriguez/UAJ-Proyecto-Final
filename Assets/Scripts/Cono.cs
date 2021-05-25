using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cono : MonoBehaviour {

		void Update () {
		Vector3 mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint (mousePos);

		Vector2 direccion = new Vector2 (mousePos.x - transform.position.x, mousePos.y - transform.position.y);
		transform.up = direccion;
	}
}
