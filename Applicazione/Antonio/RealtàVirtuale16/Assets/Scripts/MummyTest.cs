using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyTest : MonoBehaviour {

    public Transform player;

    Animator anim;

    string state = "patrol";

    public GameObject[] waypoints;

    int currentWP = 0;
    public float rootSpeed = 0.2f;
    public float speed = 1.5f;
    public float accuracyWP = 1.0f;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();
        
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 direction = player.position - this.transform.position;
        direction.y = 0;
        float angle = Vector3.Angle(direction, this.transform.forward);

        if(state == "patrol" && waypoints.Length>0)
        {

            anim.SetBool("isIdle", false);
            anim.SetBool("isWalking", true);

            if(Vector3.Distance(waypoints[currentWP].transform.position, transform.position) < accuracyWP)
            {

                currentWP++;
                if (currentWP >= waypoints.Length)
                {
                    currentWP = 0;
                }

            }

            direction = waypoints[currentWP].transform.position - transform.position;
            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rootSpeed * Time.deltaTime);


        }


        //si gira verso il player quando si accorge di lui, cioè si trova nel suo FOV di 30°
        if (Vector3.Distance(player.position, this.transform.position) < 5 && (angle<30 || state == "pursuing"))
        {

            state = "pursuing";

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rootSpeed * Time.deltaTime);

            


            //si avvicina a noi quando si accorge noi 
            if(direction.magnitude > 1)
            {

                //this.transform.Translate(0,0,0.05f);
              

                anim.SetBool("isWalking", true);
                anim.SetBool("isAttacking", false);
            }
            else
            {

                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", true);
                
            }


        }
        else
        {
         
            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
            state = "patrol";
        }

		
	}
}
