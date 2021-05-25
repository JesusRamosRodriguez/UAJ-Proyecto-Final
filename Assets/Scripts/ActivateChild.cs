using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChild : MonoBehaviour
{


    bool atope = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!GameManager.instance.CheckTutorial() && col.gameObject.CompareTag("Player"))
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            atope = true;
            TutoInicial();
        }
    }
    private void Update()
    {
        if (atope)
        {
            int actual = 0;
            GameManager.instance.SetAllowPausa(false); //no permite pausar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.instance.VistoTutorial();
                GameManager.instance.SetAllowPausa(true); //permite pausar
                this.transform.GetChild(actual).gameObject.SetActive(false);
                GameManager.instance.SetPause(false);
                Destroy(gameObject);
                Time.timeScale = 1f;
            }
        }
    }
    void TutoInicial()
    {
        int actual = 0;

        GameManager.instance.SetPause(true);
        Time.timeScale = 0f;
        GameManager.instance.SetAllowPausa(false); //no permite pausar
        actual = 0;
        this.transform.GetChild(actual).gameObject.SetActive(true); //Activar la primera imagen al empezar el nivel
        this.transform.GetChild(actual).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        atope = true;
    }
}
