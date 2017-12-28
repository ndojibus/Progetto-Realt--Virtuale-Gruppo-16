using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Classe collegata agli oggetti che possono essere legati e raccolti nell'inventario,
//Questo script va attaccato agli elementi della scena di spawn

public class SpawnRubino : MonoBehaviour {

    public GameObject Rubino;

    void Spawn()
    {
        //Posizione spaziale rispetto all'oggetto nella scena a cui viene attaccato
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
