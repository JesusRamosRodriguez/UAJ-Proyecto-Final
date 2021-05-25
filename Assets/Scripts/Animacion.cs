using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animacion : MonoBehaviour {

    Animator playerAnim;

    void Start () {
        playerAnim = GetComponent<Animator>();
        
    }

    void Update () {
        if (Input.GetKeyDown("left") || Input.GetKeyDown("a"))
            playerAnim.SetTrigger("AndaIzquierda");
        else if (Input.GetKeyDown("right") || Input.GetKeyDown("d"))
            playerAnim.SetTrigger("AndaDerecha");
        else if (Input.GetKeyDown("up") || Input.GetKeyDown("w"))
            playerAnim.SetTrigger("AndaArriba");
        else if (Input.GetKeyDown("down") || Input.GetKeyDown("s"))
            playerAnim.SetTrigger("AndaAbajo");

        else if (Input.GetKeyUp("left") || Input.GetKeyUp("a"))
            CheckAnimation();
        else if (Input.GetKeyUp("right") || Input.GetKeyUp("d"))
            CheckAnimation();
        else if (Input.GetKeyUp("up") || Input.GetKeyUp("w"))
            CheckAnimation();
        else if (Input.GetKeyUp("down") || Input.GetKeyUp("s"))
            CheckAnimation();
    }

    void CheckAnimation()
    {

		if ((Input.GetKey("left") || Input.GetKey("a")) && !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("AndaIzquierda"))
			playerAnim.SetTrigger("AndaIzquierda");
		else if ((Input.GetKey("right") || Input.GetKey("d")) && !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("AndaDerecha"))
			playerAnim.SetTrigger("AndaDerecha");
		else if ((Input.GetKey("up") || Input.GetKey("w")) && !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("AndaArriba"))
			playerAnim.SetTrigger("AndaArriba");
		else if ((Input.GetKey("down") || Input.GetKey("s")) && !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("AndaAbajo"))
			playerAnim.SetTrigger("AndaAbajo");
		if(!(Input.GetKey("left") || Input.GetKey("a")) && !(Input.GetKey("right") || Input.GetKey("d")) && !(Input.GetKey("up") || Input.GetKey("w")) && !(Input.GetKey("down") || Input.GetKey("s")))
        {
		if(playerAnim.GetCurrentAnimatorStateInfo(0).IsName("AndaIzquierda"))
			playerAnim.SetTrigger("Izquierda");
        else if ( playerAnim.GetCurrentAnimatorStateInfo(0).IsName("AndaDerecha"))
            playerAnim.SetTrigger("Derecha");
        else if ( playerAnim.GetCurrentAnimatorStateInfo(0).IsName("AndaAbajo"))
            playerAnim.SetTrigger("Abajo");
        else if ( playerAnim.GetCurrentAnimatorStateInfo(0).IsName("AndaArriba"))
            playerAnim.SetTrigger("Arriba");
        }
		
    }
}
