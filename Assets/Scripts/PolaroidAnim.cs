using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolaroidAnim : MonoBehaviour {
    public Animator anim;
    bool play = false;

	void Start () {
        anim = GetComponent<Animator>();
	}
	
	void Update () {
        if (play)
        {
            anim.Play("Polaroid");
            Invoke("Reiniciar", 2f);
        }
        
	}

    public void SetPlay(bool _play)
    {
        if (!play)
        {
            this.gameObject.SetActive(true);
            play = _play;
        }
        else
        {
            Reiniciar();
            SetPlay(_play);
        }
        
    }

    void Reiniciar()
    {
        anim.Play("New State");
        this.gameObject.SetActive(false);
        play = false;
    }
}
