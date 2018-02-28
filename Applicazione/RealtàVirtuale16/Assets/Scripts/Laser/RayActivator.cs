using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Gestisce l'apertura/chiusura della porta quando colpito dal laser
 * 
 * DA MIGLIORARE: il tempo di disattivazione fa riferimento a quando smette di essere colpito il pannello, non quando finisce l'animazione
 */
public class RayActivator : TriggerActionBase
{
    [SerializeField]
    float m_disableTime = 3f;       //IMPOSTABILE VIA EDITOR: dopo quanto tempo da quando il laser non tocca più l'interruttore si chiude la porta

    [SerializeField]
    GameObject m_doorObject;        //RICORDATI DI IMPOSTARLO VIA EDITOR: la porta

    [SerializeField]
    List<CameraTransitor> m_optionalTransitors;

    float m_actualTimer;            //timer decrescente
    bool m_activated = false;       //interruttore attivo/disattivo
    CameraTransitor m_transitorComponent;   //animatore della porta

    private AudioSource source;

    // inizializza il timer e l'animatore collegato alla porta, ricorda di impostare qual è la porta via editor
    public void Awake()
    {
        m_actualTimer = m_disableTime;
        if (m_doorObject != null)
        {
            m_transitorComponent = m_doorObject.GetComponentInChildren<CameraTransitor>();
            if (m_transitorComponent == null)
                Debug.LogError(this.name + ": " + "Impossible to find animator!");
        }
        else
            Debug.LogError(this.name + ": " + "Impossible to find door object!");

        source = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Update()
    {

        if (m_activated)
        {                                                          //se l'interruttore è attivo
            m_actualTimer -= Time.deltaTime;                                        //decrementa il timer con il tempo passato nell'ultimo frame 
            if (m_actualTimer <= 0f)
            {
                source.PlayOneShot(source.clip);
                //se il timer è scaduto
                m_activated = false;                                                //disattiva l'interruttore
                m_actualTimer = m_disableTime;                                      //resetta il timer
                m_transitorComponent.forward = !m_transitorComponent.forward;       //attiva l'animazione che chiude la porta

                if (m_optionalTransitors != null && m_optionalTransitors.Count > 0)
                    foreach (CameraTransitor transitor in m_optionalTransitors)
                        transitor.forward = !transitor.forward;

            }
        }
    }

    public override void Activate()
    {
        if (!m_activated)                                                               //se l'interruttore non è attivo
        {
            m_activated = true;                                                         //lo attivi         
            m_transitorComponent.forward = !m_transitorComponent.forward;               //attiva l'animazione che apre la porta
            source.PlayOneShot(source.clip);
            if (m_optionalTransitors != null && m_optionalTransitors.Count > 0)
                foreach (CameraTransitor transitor in m_optionalTransitors)
                    transitor.forward = !transitor.forward;
        }

        m_actualTimer = m_disableTime;                                                  //in ogni caso resetta il timer
    }
}
