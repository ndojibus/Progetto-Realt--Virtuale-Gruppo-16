using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTrigger : MonoBehaviour {

    public IntroPanel m_introPanel;

    private BoxCollider m_trigger;

    private void Awake()
    {
        m_trigger = this.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_introPanel.ActiveIntro();
            m_trigger.enabled = false;
        }
    }
}
