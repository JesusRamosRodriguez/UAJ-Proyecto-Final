using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObject : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.CompareTag("Player"))
		{
            if (this.gameObject.CompareTag ("Bombilla")) {
				GameManager.instance.PickBombilla ();
			} 
			else if (this.gameObject.CompareTag ("Carrete")) 
			{
				GameManager.instance.PickCarrete ();
			}
            Destroy(gameObject);
            
        }
	}
}
