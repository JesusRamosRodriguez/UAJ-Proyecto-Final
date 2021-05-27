using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPausa : MonoBehaviour {

	public GameObject pauseMenuUI;
	public string menu;

    float cdFlash = 0.5f; //no puede hacer flash cdFlash segundos despues de hacer click en Reanudar

	/*void Start ()
	{
		Time.timeScale = 1f;
	}*/

	void Update () 
	{
      	if (Input.GetKeyDown (KeyCode.Escape) && GameManager.instance.AllowPausa()) 
		{
			if (GameManager.instance.GameIsPaused()) 
			{
				Resume ();	
			} 
			else 
			{
				Pause ();
			}
		}
	}

    public void ResumeFlash()
    {
        GameManager.instance.SetPuedeFlash(true);
    }
		
	public void Resume ()
	{
		TelemetrySystem.Instance.levelEvent("Pausa", GameManager.instance.getLevelNumber());
		GameManager.instance.SetPuedeFlash(false);
		pauseMenuUI.SetActive (false);
		// timeScale modifica la velocidad a la que pasa el tiempo dentro del juego (va de 0 a 1)
		Time.timeScale = 1f;
		GameManager.instance.SetPause (false);
        Invoke("ResumeFlash", cdFlash);
	}

	void Pause ()
	{
		TelemetrySystem.Instance.levelEvent("Pausa", GameManager.instance.getLevelNumber());
		pauseMenuUI.SetActive (true);
		Time.timeScale = 0f; 
		GameManager.instance.SetPause (true);
	}

	public void Quit ()
	{
		Application.Quit();
	}

	public void GoToMenu ()
	{
		TelemetrySystem.Instance.levelEvent("AbandonoNivel", GameManager.instance.getLevelNumber());
		Time.timeScale = 1f;
        GameManager.instance.GoToMenu();
	}
		
}
