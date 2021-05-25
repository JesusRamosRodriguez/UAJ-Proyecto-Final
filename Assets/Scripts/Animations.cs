using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animations : MonoBehaviour {

	public Transform hijo;
    Animator anim;
    public NavMeshAgent myAgent;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update () {
        if (myAgent.velocity.y > 0.5f)
            anim.SetInteger("Direction", 1);

        else if (myAgent.velocity.y < -0.5f)
            anim.SetInteger("Direction", 2);

        else if (myAgent.velocity.x > 0.5f)
            anim.SetInteger("Direction", 3);
        else if(myAgent.velocity.x < -0.5f)
            anim.SetInteger("Direction", 4);

        transform.localPosition = new Vector3(hijo.localPosition.x, hijo.localPosition.y, hijo.localPosition.z);
            
	}
}
