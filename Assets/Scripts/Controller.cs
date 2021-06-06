using UnityEngine;

public class Controller : MonoBehaviour {

	public float moveSpeed = 6;
    float realSpeed;

	Rigidbody2D rb;
    Vector2 velocity;

    AudioSource[] fuenteAudio;
    public AudioClip objectSound;

    private void Start()
    {
        //sonido
        fuenteAudio = GetComponents<AudioSource>();

        if (GameManager.instance.MinijuegoTerminado() && GameManager.instance.NivelActual() == "Nivel1")
            transform.position = new Vector3(-16.44f, 42, 0);
        else if(GameManager.instance.MinijuegoTerminado() && GameManager.instance.NivelActual() == "Nivel2")
            transform.position = new Vector3(-16.12f, 6.97f, 0);
        else if (GameManager.instance.MinijuegoTerminado() && GameManager.instance.NivelActual() == "Nivel3")
            transform.position = new Vector3(79.83f, 23.07f, 0);
        realSpeed = moveSpeed;//Guarda la velocidad para cuando se stunee
		rb = GetComponent<Rigidbody2D> ();
    }

    void Update () {

        if(TelemetrySystem.Instance.updateFrame())
        {
            TelemetrySystem.Instance.positionEvent("PlayerPosition", this.transform.position.x, 
                this.transform.position.y, GameManager.instance.getLevelNumber());
        }
        velocity = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed;//Movimiento
    }

    void FixedUpdate() {
		
		rb.MovePosition (rb.position + velocity * Time.fixedDeltaTime); //Movimiento
    }

    public bool CompruebaE()//Comprueba si se pulsa la E
    {
        if (Input.inputString == "e")
            return true;

        else
            return false;
    }

    public void Stuned()//Para cuando se stunee al jugador
    {
        moveSpeed = 0;
        Invoke("NoStunned", 0.75f);
    }

    void NoStunned()
    {
        moveSpeed = realSpeed;
    }
    public void Spray()//Para cuando se stunee al jugador
    {
        GameObject.FindWithTag("Canvas").transform.GetChild(3).gameObject.SetActive(true);
        Invoke("NoSpray", 2);
    }
    void NoSpray()
    {
        GameObject.FindWithTag("Canvas").transform.GetChild(3).gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Guardia") && collision.gameObject.transform.parent.transform.GetChild(0).GetComponent<Detect>().LeVeo())
        {
            TelemetrySystem.Instance.singleEvent("Muerte", GameManager.instance.getLevelNumber());
            TelemetrySystem.Instance.positionEvent("MuertePosition", this.gameObject.transform.position.x,
                this.gameObject.transform.position.y, GameManager.instance.getLevelNumber());
            GameManager.instance.Pierde();
        }
        else if (collision.CompareTag("Carrete") || collision.CompareTag("Bombilla"))
        {
            fuenteAudio[0].clip = objectSound;
            fuenteAudio[0].volume = 2f;
            fuenteAudio[0].Play();
        }
        
    }
}