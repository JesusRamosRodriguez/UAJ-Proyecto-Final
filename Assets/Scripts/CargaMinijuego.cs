using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargaMinijuego : MonoBehaviour {
        void OnCollisionEnter2D (Collision2D col)
		{
            if (col.gameObject.tag == "Player"&& !GameManager.instance.MinijuegoTerminado())
            {
            

            GameManager.instance.GoToMiniJuego();
            }
			
		}

}
