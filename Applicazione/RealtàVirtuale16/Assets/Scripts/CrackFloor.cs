using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackFloor : MonoBehaviour {

    Animator anim;

    private GameObject m_FixedFloor;
    private GameObject m_CrackedFloor;
    private bool broken=false;

    private AudioSource source;
    public AudioClip crackingSound;

    private void Awake()
    {

        source = this.GetComponent<AudioSource>();
        m_FixedFloor = this.transform.Find("FixedFloor").gameObject;

        if (m_FixedFloor == null)
        {
            Debug.LogError(this.name + ": " + "Impossible to find any FixedFloor!");
        }

        m_CrackedFloor = this.transform.Find("CrackedFloor").gameObject;

        if (m_FixedFloor == null)
        {
            Debug.LogError(this.name + ": " + "Impossible to find any CrackedFloor!");


        }

        anim = m_CrackedFloor.GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError(this.name + ": " + "Impossible to find any animator!");
        }

    }

    
	
	

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !broken)
        {
            source.PlayOneShot(crackingSound, 1f);
            anim.SetBool("Break", true);
            broken = true;
            Invoke("disableUpperFloor", 0.25f);
        }
    }

    private void disableUpperFloor() {
        m_FixedFloor.SetActive(false);
    }
}
