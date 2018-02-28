using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {

        AudioSource footstep = this.GetComponent<AudioSource>();
        if (footstep != null)
        {   if (!footstep.isPlaying)
                footstep.PlayOneShot(footstep.clip);
        }
        else
            Debug.LogError(this.name + ": Impossible to find audio source!");
    }
}
