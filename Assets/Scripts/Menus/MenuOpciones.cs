using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuOpciones : MonoBehaviour {

    public AudioMixer audioMixer;
	public int controlIndex;

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			this.gameObject.SetActive (false);
		}
	}

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("Volume", volume);

    }

	public void ToogleFullScreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}

	public void SetControlIndex (int optionIndex)
	{
		controlIndex = optionIndex;

		/*	
		 * 0:Teclado
		 * 	1:Teclado ALT
		 *  2:Mando
		 * 	3:Mando ALT
		 */

		switch (controlIndex) 
		{
		case(0):
			Debug.Log ("Teclado" + controlIndex);
			break;
		case(1):
			Debug.Log ("Teclado ALT" + controlIndex);
			break;
		case(2):
			Debug.Log ("Mando" + controlIndex);
			break;
		case(3):
			Debug.Log ("Mando ALT" + controlIndex);
			break;
		default:
			Debug.Log ("ERROR");
			break;
		}
	}
		
}
