using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepTrigger : MonoBehaviour {


    private AudioSource footstep;

    private float timerReset = 0.18f;

    private bool once= false;



    private void Awake()
    {
        footstep = this.GetComponent<AudioSource>();
        if (footstep == null)
        {
            Debug.LogError(this.name + ": Impossible to find audio source!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!once)
        {
            footstep.PlayOneShot(footstep.clip);
            once = true;
            Invoke("ResetFootStep", timerReset);
        }
        
 
    }

    private void ResetFootStep()
    {
        once = false;
    }
}
