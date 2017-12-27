using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRubino : MonoBehaviour {

    public GameObject Rubino;

    void Spawn()
    {

        Debug.Log("Nuovo rubino");

            Vector3 itemPos = new Vector3(this.transform.position.x,
                                           this.transform.position.y ,
                                           this.transform.position.z);
            Instantiate(Rubino, itemPos, Quaternion.identity);

       
    }

    // Use this for initialization
    void Start () {

        Spawn();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
