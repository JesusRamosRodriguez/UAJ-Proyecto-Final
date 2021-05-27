using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour {

	public Transform[] patrolPoints;
	public float speed;
	Transform currentPatrol;
	int currentPatrolIndex;
	Rigidbody2D rb;
    private NavMeshAgent agent;
    float realSpeed;
    GameObject go;
    public bool acudeCamara = false;

    Transform camera;

    public bool patrulla = true;

    //sonido detección
    AudioSource fuenteAudio;
    bool puedeSonar = true;
    public AudioClip detectadoAudio;

    void Start ()
	{
		agent = gameObject.GetComponent<NavMeshAgent>();
		realSpeed = agent.speed;//Velocidad real 
		rb = GetComponent<Rigidbody2D> ();
		currentPatrolIndex = 0;//Punto de patrulla
		currentPatrol = patrolPoints [currentPatrolIndex];
        agent.updateRotation = false;
        go = GameObject.FindWithTag("Player");

        //sonido
        fuenteAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        TelemetrySystem.Instance.positionEvent("GuardiaPosition", this.gameObject.transform.position.x,
            this.gameObject.transform.position.y, GameManager.instance.getLevelNumber());
        // Primero comprueba que sea un guardia y no un famoso el objeto que tiene este script
        if (!this.gameObject.CompareTag("Famoso") && transform.GetChild(0).GetComponent<Detect>().LeVeo()) 
        {
            //sonido
            if (puedeSonar)
                Sonido();


            patrulla = false;
            acudeCamara = false;

            //Vector direccion hacia donde se debe mover el guardia
            Vector2 dir = new Vector2(go.transform.position.x - transform.position.x, go.transform.position.y - transform.position.y);
            agent.SetDestination(go.transform.position);

            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, Vector2.SignedAngle(Vector2.up, dir), 0.2f));//Rotaicion del guardia
        }
        else if(acudeCamara)
        {
            //sonido
            if (puedeSonar)
                Sonido();


            Vector2 dir = new Vector2(camera.position.x - transform.position.x, camera.position.y - transform.position.y);

            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, Vector2.SignedAngle(Vector2.up, dir), 0.2f));//Rotaicion del guardia

            if (Vector3.Distance(transform.position, camera.position) < 0.1f)
            {
                acudeCamara = false;
                Patrulla();
            }
        }
        else
        {
            if (!patrulla)
            {
                Invoke("Patrulla", 2);
                agent.SetDestination(go.transform.position);
            }
            if (patrulla)
            {
                //Vector direccion hacia donde se debe mover el guardia
                Vector2 dir = new Vector2(currentPatrol.position.x - transform.position.x, currentPatrol.position.y - transform.position.y);


                agent.SetDestination(currentPatrol.position);

                if (Vector3.Distance(transform.position, currentPatrol.position) < 0.1f)
                {//Cambia el punto de patrulla actual
                    if (currentPatrolIndex + 1 < patrolPoints.Length)
                        currentPatrolIndex++;
                    else
                    {
                        currentPatrolIndex = 0;
                    }
                    currentPatrol = patrolPoints[currentPatrolIndex];

                }
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, Vector2.SignedAngle(Vector2.up, dir), 0.2f));//Rotaicion del guardia
            }
        }
    }

    public void Stunned()
    {
        agent.isStopped = true;
        Invoke("NoStunned", 2);
    }
    void NoStunned()
    {
        agent.isStopped = false;
    }

    public void Spray()
    {
        agent.isStopped = true;
        Invoke("NoStunned", 0.5f);
    }

     void Patrulla()
	 { 
        patrulla = true;
        //sonido
        puedeSonar = true;
     }

    public void AcudeACamara(Transform camara)
    {
        patrulla = false;
        acudeCamara = true;
        agent.SetDestination(camara.position);
        camera = camara;
    }

    public void Sonido()
    {
        puedeSonar = false;
        fuenteAudio.clip = detectadoAudio;
        fuenteAudio.Play();
    }
}