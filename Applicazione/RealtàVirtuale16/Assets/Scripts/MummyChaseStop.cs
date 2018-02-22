using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyChaseStop : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") {
            PatrolList mummyPatrol = other.GetComponent<PatrolList>();
            mummyPatrol.canChase = false;
        }
    }

}
