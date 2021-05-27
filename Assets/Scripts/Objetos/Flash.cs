using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    //Animación
    Animator playerAnim;
    

    //sonido
    public AudioClip flashSound;
    public AudioClip cameraSound;
    public AudioClip goodPhotoSound;
    public AudioClip failPhotoSound;
    AudioSource camAudio, fotoAudio;

    //Puntos famosos
    public int fotoFrentePts = 500, fotoEspaldaPts = 100;

    //Feedback Famosos
    public GameObject polaroid;


    private GameObject ConoFlash;
    private GameObject ConoFoto;
    private Rigidbody2D rbConoFlash;
    private Rigidbody2D rbConoFoto;


    void Start()
    {
        //Animación
        playerAnim = GetComponent<Animator>();

        //sonido
        //sonido
        AudioSource[] audios = GetComponents<AudioSource>();
        camAudio = audios[0];
        fotoAudio = audios[1];


        ConoFlash = this.transform.GetChild(0).gameObject;
        ConoFoto = this.transform.GetChild(1).gameObject;
        rbConoFlash = ConoFlash.GetComponent<Rigidbody2D>();
        rbConoFoto = ConoFoto.GetComponent<Rigidbody2D>();

        Invisible();
    }

    void Update()
    {
		if (!GameManager.instance.GameIsPaused() && GameManager.instance.PuedeFlash()) // Si el juego no está en PAUSA
        {
			if (Input.GetKeyDown(KeyCode.Mouse0) && GameManager.instance.bombillas > 0)
            {
                VisibleFlash();
            }

            else if (Input.GetKeyDown(KeyCode.Mouse1) && GameManager.instance.carretes > 0)
            {
                VisibleFoto();
            }

            if (  Input.GetKeyUp(KeyCode.Mouse1)  && GameManager.instance.carretes > 0)
            {

                Invisible();
                if (ConoFoto.GetComponent<Raycast>().LeVeo() && ConoFoto.GetComponent<Raycast>().Frente() && ConoFoto.GetComponent<Raycast>().FotoAQuien().CompareTag("Famoso"))//Si esta de frente y dentro llama a stun
                {
                    FamosoFrente();
                }
                else if (ConoFoto.GetComponent<Raycast>().LeVeo() && !ConoFoto.GetComponent<Raycast>().Frente() && ConoFoto.GetComponent<Raycast>().FotoAQuien().CompareTag("Famoso"))
                {
                    FamosoEspaldas();
                }
                else if (ConoFoto.GetComponent<Raycast>().LeVeo() && ConoFoto.GetComponent<Raycast>().FotoAQuien().CompareTag("Arnold") && ConoFoto.GetComponent<Raycast>().FotoAQuien().GetComponent<Arnold>().IsFace())
                {
                    FamosoFrente();
                }
                else if (ConoFoto.GetComponent<Raycast>().LeVeo() && ConoFoto.GetComponent<Raycast>().FotoAQuien().CompareTag("Arnold") && !ConoFoto.GetComponent<Raycast>().FotoAQuien().GetComponent<Arnold>().IsFace())
                {
                    FamosoEspaldas();
                }
                else    //Ha fallado la foto
                {
                    //Sonido de error
                    fotoAudio.clip = failPhotoSound;
                    fotoAudio.volume = 0.3f;
                    fotoAudio.Play();
                }

                if (ConoFoto.GetComponent<Raycast>().LeVeo() && ConoFoto.GetComponent<Raycast>().FotoAQuien().CompareTag("Guardia"))
                    TelemetrySystem.Instance.singleEvent("FotoGuardia", GameManager.instance.getLevelNumber());

                TelemetrySystem.Instance.singleEvent("FotoUsada", GameManager.instance.getLevelNumber());
                GameManager.instance.carretes--;
                //sonido
                camAudio.clip = cameraSound;
                camAudio.volume = 0.5f;
                camAudio.Play();
            }
			if(Input.GetKeyUp(KeyCode.Mouse0) && GameManager.instance.bombillas > 0)
            {
                Invisible();
                if (ConoFlash.GetComponent<Raycast>().LeVeo() && ConoFlash.GetComponent<Raycast>().Frente())//Si esta de frente y dentro llama a stunn
                    ConoFlash.GetComponent<Raycast>().Stunn();

                TelemetrySystem.Instance.singleEvent("FlashUsado", GameManager.instance.getLevelNumber());
                GameManager.instance.bombillas--;
                //sonido
                camAudio.clip = flashSound;
                camAudio.volume = 0.25f;
                camAudio.Play();
            }
        }

    }

    void Invisible()
    {
        ConoFlash.SetActive(false);
        ConoFoto.SetActive(false);
        rbConoFlash.Sleep();
        rbConoFoto.Sleep();
    }

    void VisibleFoto()
    {
        ConoFoto.SetActive(true);
        rbConoFoto.WakeUp();
        //Animación con la cámara
        playerAnim.SetTrigger("Camara");
    }

    void VisibleFlash()
    {
        ConoFlash.SetActive(true);
        rbConoFlash.WakeUp();
        //Animación con la cámara
        playerAnim.SetTrigger("Camara");
    }

    void FamosoFrente()
    {
        //Imagen feedback
        polaroid.transform.GetChild(0).GetComponent<PolaroidAnim>().SetPlay(true);
        //Sonido
        fotoAudio.clip = goodPhotoSound;
        fotoAudio.volume = 0.75f;
        fotoAudio.Play();
        //Puntos
        GameManager.instance.SumaPuntos(fotoFrentePts, "opcional");
    }

    void FamosoEspaldas()
    {
        //Imagen feedback
        polaroid.transform.GetChild(1).GetComponent<PolaroidAnim>().SetPlay(true);
        //Sonido
        fotoAudio.clip = goodPhotoSound;
        fotoAudio.volume = 0.75f;
        fotoAudio.Play();
        //Puntos
        GameManager.instance.SumaPuntos(fotoEspaldaPts, "opcional");
    }
}
