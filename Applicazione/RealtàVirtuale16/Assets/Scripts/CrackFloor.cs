using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackFloor : MonoBehaviour {

    Animator anim;

    // Use this for initialization
    void Start () {

        anim = GetComponent<Animator>();

        if(anim== null)
        {
            Debug.LogError(this.name + ": " + "Impossible to find any animator!");
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            anim.SetBool("Break", true);

        }
    }
}
