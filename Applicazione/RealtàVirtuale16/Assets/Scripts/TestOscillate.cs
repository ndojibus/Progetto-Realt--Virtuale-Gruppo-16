using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOscillate : MonoBehaviour {

    private float min;
    private float max;


	// Use this for initialization
	void Start () {
        min = transform.position.x;
        max = transform.position.x - 8.2f;
		
	}
	
	// Update is called once per frame
	void Update () {

        
            transform.localPosition = new Vector3(Mathf.PingPong(Time.time, 1.65f)+0.18f, transform.localPosition.y , transform.localPosition.z);
        

    }
}
