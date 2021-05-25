using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
	int ptosLoot = 1000;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayLoot.PlaySound();
            GameManager.instance.loot++;
			GameManager.instance.SumaPuntos (ptosLoot, "loot");
            Destroy(this.gameObject);
            TelemetrySystem.Instance.addEvent("Coleccionable", GameManager.instance.getLevelNumber());
        }
    }
}
