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

    [SerializeField]
    float m_rayStartingHigh = 0f;                //IMPOSTABILE VIA EDITOR: altezza del raggio laser

    [SerializeField]
    LayerMask m_ignoreLayers;             //IMPOSTABILE VIA EDITOR: seleziona i layer da ignorare

    float m_rayLenght = 0f;                          //lunghezza attuale del laser

    private bool m_laserActive = false;

    GameObject m_laserObject;
    LineRenderer m_laserLine;
    ParticleSystem m_particleLaser;
    GameObject m_light;
    GameObject m_particle;

    Vector3 m_collisionPoint;
    Vector3 m_newStartingPosition;

    public AudioClip rotationSound;
    private AudioSource source;
    private bool isPlaying = false;

    public bool laserActive { get { return m_laserActive; } set { m_laserActive = value; } }

    public bool controlled{ get { return m_controlled; }
                            set { m_controlled = value; }
    }

    // Inizializza l'oggetto laser prendendo il primo figlio (se ne aggiungi altri ricorda questo dettaglio)
    void Awake() {
        m_laserObject = transform.GetChild(0).gameObject;
        if (m_laserObject == null)
            Debug.LogError(this.name + ": " + "Impossible to find laserObject!");

        m_laserLine = transform.GetChild(0).GetComponent<LineRenderer>();
        if (m_laserLine == null)
            Debug.LogError(this.name + ": " + "Impossible to find line Renderer!");
        m_particleLaser = transform.GetChild(0).GetComponent<ParticleSystem>();
        if (m_particleLaser == null)
            Debug.LogError(this.name + ": " + "Impossible to find Particle system for the laser!");

        m_light = transform.GetChild(0).GetChild(0).gameObject;
        if (m_light == null)
            Debug.LogError(this.name + ": " + "Impossible to find light!");

        //m_particle = transform.GetChild(0).GetChild(1).gameObject;
        //if (m_particle == null)
        //    Debug.LogError(this.name + ": " + "Impossible to find particle!");

        if (m_ignoreLayers == null)
            Debug.LogError(this.name + ": " + "Impossible to find layermask!");


        source = this.GetComponent<AudioSource>();

    }

    

    // Nell'update chiamo 3 funzioni, controlla quello che fanno nella loro dichiarazione sotto
    void Update()
    {
        m_newStartingPosition = this.transform.position + this.transform.forward * m_rayDistance;
        m_newStartingPosition += this.transform.up * m_rayStartingHigh;
        if (!m_laserObject.activeSelf && m_laserActive)
            m_laserObject.SetActive(true);
        else if (m_laserObject.activeSelf && !m_laserActive)
            m_laserObject.SetActive(false);

        if (m_laserActive)
        {
            if (m_controlled)
                InputManager();
            RayCast();
            ChangeLaser();
        }
    }

    // Funzione usata per disegnare il gizmo della lunghezza attuale reale del raggio laser, per vederlo clicca "Gizmos" in alto a destra dalla finestra del gioco
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(m_newStartingPosition, this.transform.forward * m_rayLenght);
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

            if (!isPlaying)
            {
                source.Play();
                source.loop = true;
                isPlaying = true;
            }
                
        }
        else
        {
            source.Stop();
            isPlaying = false;
        }

    }

    // Emette il raggio e se incontra un interruttore lo attiva
    private void RayCast() {
        RaycastHit hit;     //contiene informazioni sulla collisione del raggio
        if (Physics.Raycast(m_newStartingPosition, this.transform.forward, out hit, 10f, m_ignoreLayers))  //se raggio che va dalla posizione dell'oggetto verso avanti colpisce qualcosa
        {
            m_rayLenght = Vector3.Distance(m_newStartingPosition, hit.point);         //imposta la lunghezza del raggio come la distanza fra l'oggetto e il punto di collisione    

            var activator = hit.collider.gameObject.GetComponent<TriggerActionBase>();
            if (activator != null)                                                                      //se l'oggetto colpito dal raggio è un activator
                activator.Activate();

            m_collisionPoint = hit.point;


        }
        else
        {
            m_collisionPoint = this.transform.position + this.transform.forward * m_maximumRayLenght;
            m_collisionPoint += this.transform.up * m_rayStartingHigh;                                           //se no imposta la lunghezza del raggio a quella massima
            m_rayLenght = m_maximumRayLenght;
        }
    }

    /* Funzione che cambia l'aspetto del laser visualizzato (VECCHIA VERSIONE DEL COMMENTO)
     * 
     * UN APPUNTO: il cilindro standard di unity è 2x2 ed ha il pivot nel punto centrale e non alla base,
     * per questo oltre a scalarlo di una dimensione pari alla lunghezza del raggio/2, e non solo la lunghezza del raggio,
     * va anche traslato in avanti di newPosition, ovvero metà della lunghezza del raggio
     */
    private void ChangeLaser()
    {
        
        m_laserLine.transform.position = m_newStartingPosition;
        m_laserLine.SetPosition(0, new Vector3(0, 0, 0));
        m_laserLine.SetPosition(1, m_laserLine.transform.InverseTransformPoint(m_collisionPoint));

        m_particleLaser.startLifetime = Vector3.Distance(m_particleLaser.transform.position, m_collisionPoint) / m_particleLaser.startSpeed;
        m_light.transform.position = m_collisionPoint - transform.forward * 0.1f;
        //m_particle.transform.localPosition = collisionPoint;


    }


}
