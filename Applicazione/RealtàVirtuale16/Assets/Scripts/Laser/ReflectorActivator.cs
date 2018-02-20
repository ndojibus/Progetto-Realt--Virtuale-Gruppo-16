using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorActivator : TriggerActionBase
{
    [SerializeField]
    float m_disableTime = 3f;       //IMPOSTABILE VIA EDITOR: dopo quanto tempo da quando il laser non tocca più l'interruttore si chiude la porta

    [SerializeField]
    InteractableReflectingLaser m_reflectingLaser;        //RICORDATI DI IMPOSTARLO VIA EDITOR: la porta

    float m_actualTimer;            //timer decrescente
    bool m_activated = false;       //interruttore attivo/disattivo

    // inizializza il timer e l'animatore collegato alla porta, ricorda di impostare qual è la porta via editor
    public void Awake()
    {
        m_actualTimer = m_disableTime;
        if (m_reflectingLaser == null)
            Debug.LogError(this.name + ": " + "Impossible to find laser object!");
    }

    // Update is called once per frame
    public void Update()
    {

        if (m_activated)
        {                                                          //se l'interruttore è attivo
            m_actualTimer -= Time.deltaTime;                                        //decrementa il timer con il tempo passato nell'ultimo frame 
            if (m_actualTimer <= 0f)
            {                                              //se il timer è scaduto
                m_activated = false;                                                //disattiva l'interruttore
                m_actualTimer = m_disableTime;                                      //resetta il timer
                m_reflectingLaser.equiped = false;
            }
        }
    }

    public override void Activate()
    {
        if (!m_activated)                                                               //se l'interruttore non è attivo
        {
            m_activated = true;                                                         //lo attivi         
            m_reflectingLaser.equiped = true;                                          //attiva il laser
        }

        m_actualTimer = m_disableTime;                                                  //in ogni caso resetta il timer
    }
}
