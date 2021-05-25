using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rango : MonoBehaviour {

	int puntuacion = 0;
	GameObject rank;

	/*void Awake()
	{
		puntuacion = GameManager.instance.GetPuntos ();
		//puntuacion = 55789;
	}*/

	void Start()
	{
        puntuacion = GameManager.instance.GetPuntos();

        if (puntuacion >= 10000) // S
		{	
			rank = this.transform.GetChild (0).gameObject;
			rank.SetActive (true);
			Debug.Log ("S");
		}
		else if(puntuacion < 10000 && puntuacion >= 8500) // A
		{
			rank = this.transform.GetChild (1).gameObject;
			rank.SetActive (true);
			Debug.Log ("A");
		}
		else if(puntuacion < 8500 && puntuacion >= 7000) // B
		{
			rank = this.transform.GetChild (2).gameObject;
			rank.SetActive (true);
			Debug.Log ("B");
		}
		else if(puntuacion < 7500 && puntuacion >= 5000) // C
		{
			rank = this.transform.GetChild (3).gameObject;
			rank.SetActive (true);
			Debug.Log ("C");
		}
		else if(puntuacion < 5000) // D
		{
			rank = this.transform.GetChild (4).gameObject;
			rank.SetActive (true);
			Debug.Log ("D");
		}
		else
			Debug.Log ("ERROR   "+ puntuacion);
	}
}
