using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    public TutorialPanel m_tutorialPanel;

    private BoxCollider m_trigger;

    private void Awake()
    {
        m_trigger = this.GetComponent<BoxCollider>();
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== "Player")
        {
            m_tutorialPanel.ActiveTutorial();
            m_trigger.enabled = false;
        }
    }

}
