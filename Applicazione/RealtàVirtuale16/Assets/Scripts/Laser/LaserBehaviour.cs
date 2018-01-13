using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Classe che determina il comportamento del laser
 * 
 * DA MIGLIORARE: come rappresentare visivamente il laser, per adesso è un cilindro con materiale emettente
 */
public class LaserBehaviour : MonoBehaviour {
    [SerializeField]
    float m_maximumRayLenght = 10f;             //IMPOSTABILE VIA EDITOR: lunghezza massima del raggio

    [SerializeField]
    float m_angularVelocity = 90f;              //IMPOSTABILE VIA EDITOR: velocità angolare del laser (di quanti gradi si sposta in un secondo)

    [SerializeField]
    bool m_controlled = false;                  //IMPOSTABILE VIA EDITOR: indica se puoi controllare il laser o no

    [SerializeField]
    float m_rayDistance = 0.2f;                 //IMPOSTABILE VIA EDITOR: distanza dal gameobject padre del laser

    [SerializeField]
    float m_rayThickness = 0.2f;                //IMPOSTABILE VIA EDITOR: spessore del raggio laser

    float m_rayLenght;                          //lunghezza attuale del laser

    GameObject m_laser;                         //gameobject che rappresenta un cilindro con materiale emettente per fare il laser, usato per prova
    

    public bool controlled{ get { return m_controlled; }
                            set { m_controlled = value; }
    }

    // Inizializza l'oggetto laser prendendo il primo figlio (se ne aggiungi altri ricorda questo dettaglio)
    void Awake() {
        m_laser = transform.GetChild(0).gameObject;
        if (m_laser == null)
            Debug.LogError(this.name + ": " + "Impossible to find laser!");
        
    }

    // Nell'update chiamo 3 funzioni, controlla quello che fanno nella loro dichiarazione sotto
    void Update()
    {
        if(!m_laser.activeSelf)
            m_laser.SetActive(true);
        if (m_controlled)
            InputManager();
        RayCast();
        ChangeLaser();
    }

    // Funzione usata per disegnare il gizmo della lunghezza attuale reale del raggio laser, per vederlo clicca "Gizmos" in alto a destra dalla finestra del gioco
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(this.transform.position, this.transform.forward * m_rayLenght);
    }

    /* Funzione che gestisce la rotazione del laser in base alla pressione delle frecce destra-sinistra o i tasti a-d
     * 
     *  transform.RotateAround ruota un il laser rispetto al vettore up del gameobject di un angolo dato dalla velocità angolare * il tempo impiegato dall'ultimo frame
     */
    private void InputManager() {
        if (Input.GetButton("Horizontal"))
        {
            if (Input.GetAxis("Horizontal") < 0)
                this.transform.RotateAround(this.transform.position, this.transform.up, -m_angularVelocity * Time.deltaTime);

            if (Input.GetAxis("Horizontal") > 0)
                this.transform.RotateAround(this.transform.position, this.transform.up, m_angularVelocity * Time.deltaTime);
        }
    }

    // Emette il raggio e se incontra un interruttore lo attiva
    private void RayCast() {
        RaycastHit hit;     //contiene informazioni sulla collisione del raggio
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit))  //se raggio che va dalla posizione dell'oggetto verso avanti colpisce qualcosa
        {
            m_rayLenght = Vector3.Distance(this.transform.position, hit.point);         //imposta la lunghezza del raggio come la distanza fra l'oggetto e il punto di collisione    

            var activator = hit.collider.gameObject.GetComponent<TriggerActionBase>();
            if (activator != null)                                                      //se l'oggetto colpito dal raggio è un activator
                activator.Activate();                                                   //instanzia la funzione Activate sull'interruttore
        }
        else
            m_rayLenght = m_maximumRayLenght;                                           //se no imposta la lunghezza del raggio a quella massima
    }

    /* Funzione che cambia l'aspetto del laser visualizzato
     * 
     * UN APPUNTO: il cilindro standard di unity è 2x2 ed ha il pivot nel punto centrale e non alla base,
     * per questo oltre a scalarlo di una dimensione pari alla lunghezza del raggio/2, e non solo la lunghezza del raggio,
     * va anche traslato in avanti di newPosition, ovvero metà della lunghezza del raggio
     */
    private void ChangeLaser()
    {
        Vector3 newPosition = new Vector3(0, 0, m_rayLenght / 2 + m_rayDistance);
        Vector3 newScale = new Vector3(m_rayThickness, m_rayLenght / 2 - m_rayDistance, m_rayThickness);
        m_laser.transform.localPosition = newPosition;
        m_laser.transform.localScale = newScale;
    }


}
