﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spuntoni : MonoBehaviour {


    public float velocity = 1f;

    private GameObject FilaSpuntoni;
    public AudioClip spuntoniSound;
    private AudioSource source;
    private bool once = false;
    public bool sound;



    // Use this for initialization
    void Start () {

        FilaSpuntoni = this.transform.Find("FilaSpuntoni").gameObject;

        source = this.GetComponent<AudioSource>();
        
        
        //manca il controllo


    }
	
	// Update is called once per frame
	void Update () {

        FilaSpuntoni.transform.localPosition = new Vector3(Mathf.PingPong(Time.time*velocity, 1.65f), FilaSpuntoni.transform.localPosition.y, FilaSpuntoni.transform.localPosition.z);
        
        if (sound && FilaSpuntoni.transform.localPosition.x <= 0.1 &&!once)
        {
            source.PlayOneShot(spuntoniSound);
            once = true;
        }else
    {
            once = false;
        }
        

    }

    

    
}
