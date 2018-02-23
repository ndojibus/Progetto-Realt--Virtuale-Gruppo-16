using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMorto : MonoBehaviour {

    public UIManager m_UIManager;
    


    private void OnTriggerEnter(Collider other)
    {

        
        if (other.tag == "Player")
        {
            m_UIManager.DeathScreen();
        }
    }
}
