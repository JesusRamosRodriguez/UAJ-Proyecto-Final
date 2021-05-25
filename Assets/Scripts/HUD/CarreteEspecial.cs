using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarreteEspecial : MonoBehaviour {

	void Update () {
        // Desactiva el carrete especial en el HUD una vez que es usado
        if(GameManager.instance.CarreteEspecial() != 1)
            this.gameObject.SetActive(false);
    }
}
