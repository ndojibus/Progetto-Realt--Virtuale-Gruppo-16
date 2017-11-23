using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Ho creato questa classe astratta nel caso dovessimo avere più comportamenti attivati dal laser,
 * devi semplicemente far ereditare da questa classe lo script che causerà l'azione attivata dal laser
 */
public abstract class TriggerActionBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract void Activate();
}
