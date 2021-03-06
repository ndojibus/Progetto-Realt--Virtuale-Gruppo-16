using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowCamera : MonoBehaviour
{

    public Transform followTarget;

    Vector3 offset;

    public void Start()
    {
        transform.position = new Vector3(followTarget.position.x, 10, followTarget.position.z);
        offset = transform.position - followTarget.position;
        
    }

    void LateUpdate()
    {
        if (followTarget != null)
        {
            transform.position = followTarget.position + offset;
            //transform.rotation = Quaternion.LookRotation(-followTarget.up, followTarget.forward);
        }

    }
}
