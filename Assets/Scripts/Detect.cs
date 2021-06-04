using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour {

	public bool leVeo = false;//Si el guardia puede ver al jugador
	public bool dentro = false;//Si el jugador esta dentro del campo de vision(No significa que le vea)

	public LayerMask whatToHit;//Que tiene encuenta el raycast(Paredes y jugador)

	Rigidbody2D rb;

    void Start()
	{
		rb = GetComponentInParent<Rigidbody2D> ();     
    }

	private void FixedUpdate()
	{
		GameObject go = GameObject.FindWithTag("Player");
		RaycastHit2D hit;

		Vector2 dir = new Vector2(go.transform.position.x - transform.position.x, go.transform.position.y - transform.position.y).normalized;//Vector direccion que usa el raycast

		if (dentro)//Si esta dentro se lanza un raycast
		{
			float dist = Vector2.Distance(go.transform.position, transform.position);//Vector distancia del raycast

			hit = Physics2D.Raycast(transform.position, dir, dist, whatToHit);//raycast

			if (hit.collider.CompareTag("Player"))//Si colisiona con el jugador leVeo = true
			{
				leVeo = true;
				Debug.DrawRay(transform.position, dir, Color.green);
            }
            else
			{
				leVeo = false;
				Debug.DrawRay(transform.position, dir, Color.red);
			}
		}
		else
		{
			leVeo = false;
			Debug.DrawRay(transform.position, dir, Color.red);
		}
	}
	private void OnTriggerEnter2D(Collider2D col)
	{
		GameObject go = col.gameObject;

		if (go.CompareTag("Player"))
        {
			dentro = true;
			TelemetrySystem.Instance.singleEvent("GuardiaDetectaJugador", GameManager.instance.getLevelNumber());
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		GameObject go = col.gameObject;

		if (go.CompareTag("Player"))
			dentro = false;
	}

	public bool LeVeo()
	{
		return leVeo;
	}

	public bool Dentro()
	{
		return dentro;
	}
}