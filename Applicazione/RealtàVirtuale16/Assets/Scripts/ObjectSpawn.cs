using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour {

    [SerializeField]
    private List<GameObject> m_objectList;

    [SerializeField]
    private string m_triggerTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == m_triggerTag)
        {
            foreach (GameObject obj in m_objectList)
                obj.SetActive(!obj.activeSelf);
            this.gameObject.SetActive(false);
        }
    }
}
