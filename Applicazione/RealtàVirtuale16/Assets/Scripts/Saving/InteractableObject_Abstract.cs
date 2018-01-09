using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public abstract class InteractableObject_Abstract : PersistentData
{
    [SerializeField]
    protected string m_description;               //il testo descrittivo sulla GUI

    [SerializeField]
    protected float m_cameraSwitchTime;      //quanto deve rimanere in transizione la camera prima di switchare al ritorno

    [SerializeField]
    protected int m_transitorID = 0;              //l'ID del cameratransitor a cui fa riferimento questo oggetto

    protected bool m_camerasInTransition = false;
    protected Camera m_mainCamera;
    protected Camera m_objectCamera;
    protected CameraTransitor[] m_transitors;
    protected CameraTransitor m_transitor;
    protected ThirdPersonUserControl m_userControl;
    protected GameObject m_player;
    protected Canvas m_hudCanvas;
    protected GameObject m_inventoryPanel;
    protected GameObject m_inspectPanel;
    protected Text m_inspectText;
    protected GameObject m_descriptionPanel;
    protected Text m_descriptionText;
    protected GameObject m_continuePanel;
    protected Text m_continueText;

    protected PlayerInventory m_inventory;

    protected bool m_inspectMode;
    protected bool m_equiped;
    


    float m_timer;

    protected void Awake()
    {
        base.Awake();
        //aggiunto io test
        GameObject canvas = GameObject.Find("HUDCanvas");
        if (canvas != null)
        {

            m_inspectPanel = canvas.transform.Find("InspectPanel").gameObject;
            m_inspectText = m_inspectPanel.GetComponentInChildren<Text>();
            if (m_inspectPanel == null || m_inspectText== null)
                Debug.LogError("Impossible to find inspect!");
            else
            {
                m_inspectPanel.SetActive(false);

            }

            m_descriptionPanel = canvas.transform.Find("DescriptionPanel").gameObject;
            m_descriptionText = m_descriptionPanel.GetComponentInChildren<Text>();
            if (m_descriptionPanel == null || m_descriptionText == null)
                Debug.LogError("Impossible to find description!");
            else
            {
                m_descriptionPanel.SetActive(false);

            }

            m_continuePanel = canvas.transform.Find("ContinuePanel").gameObject;
            m_continueText = m_continuePanel.GetComponentInChildren<Text>();
            if (m_continuePanel == null || m_continueText == null)
                Debug.LogError("Impossible to find continue!");
            else
            {
                m_continuePanel.SetActive(false);

            }

            m_inventory = canvas.GetComponentInChildren<PlayerInventory>();
            if (m_inventory == null)
                Debug.LogError("Impossible to find a PlayerInventory!");
        }
        else
            Debug.LogError("Impossible to find a canvas!");

        m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (m_mainCamera == null)
            Debug.LogError("Impossible to find MainCamera!");

        m_objectCamera = GetComponentInChildren<Camera>();
        if (m_objectCamera == null)
            Debug.LogError("Impossible to find laser camera!");

        m_player = GameObject.FindGameObjectWithTag("Player");
        if (m_player != null)
        {
            m_userControl = m_player.GetComponent<ThirdPersonUserControl>();
            if (m_userControl == null)
                Debug.LogError("Impossible to find ThirdPersonUserControl!");

        }
        else
            Debug.LogError("Impossible to find a player!");

        m_inspectMode = false;

        m_transitors = GetComponentsInChildren<CameraTransitor>();
        if (m_transitors.Length > 0)
        {
            
        }
        else
            Debug.LogError("Impossible to find any transitor attached to this object children!");



    }

    // Use this for initialization
    protected void Start () {
        m_timer = m_cameraSwitchTime;

        int i;
        for (i = 0; i < m_transitors.Length; i++)
        {

            if (m_transitors[i].transitorID == m_transitorID)
                break;
        }
        if (i < m_transitors.Length)
            m_transitor = m_transitors[i];
        else
            Debug.LogError("Impossible to find camera transitor!");     
    }

     protected void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !m_inspectMode
           && (Vector3.Angle(other.transform.forward, this.transform.position - other.transform.position) < 90f))
        {
            SetAction();
        }
        //Se c'è collisione con il player e viene premuto e si passa in inspect mode
        if (other.tag == "Player" && !m_inspectMode && Input.GetKeyDown(KeyCode.E) 
            && (Vector3.Angle(other.transform.forward, this.transform.position - other.transform.position) < 90f) )
        {
            transitionInActions();

            m_transitor.forward = !m_transitor.forward;
            SetAction();
        }
    }


    //Da Cambiare 
    protected void OnTriggerExit(Collider other)
    {

        toggleInspectPanel(false);
        
    }

    //Attiva/disattiva il pannello Inspect
    protected void toggleInspectPanel(bool active)
    {

        if(m_inspectPanel.activeSelf!= active)
        {
            m_inspectPanel.SetActive(!m_inspectPanel.activeSelf);

        }

    }

    protected void toggleDescriptionPanel(bool active)
    {

        if (m_descriptionPanel.activeSelf != active)
        {
            m_descriptionPanel.SetActive(!m_descriptionPanel.activeSelf);
        }

    }

    //Attiva/disattiva il inspect mode, cioè pannello descrizione e pannello continue
    protected void ToggleInspectMode(bool active)
    {

        

        if(m_continuePanel.activeSelf != active)
        {
            m_continuePanel.SetActive(!m_continuePanel.activeSelf);
  
        }

        if (m_descriptionPanel.activeSelf != active)
        {
            m_descriptionText.text = m_description;
            m_descriptionPanel.SetActive(!m_descriptionPanel.activeSelf);
            

        }

        m_inspectMode = active;

    }

    protected abstract void SetAction();

    // Update is called once per frame
    protected void Update()
    {

        if (m_inspectMode && Input.GetKeyDown(KeyCode.Escape))
        {
            
            
            transitionOutActions();

            m_transitor.forward = !m_transitor.forward;
            SetAction();
        }

        if (m_camerasInTransition)
        {
  
            m_timer -= Time.deltaTime;

            if (m_timer <= 0f)
            {


                m_timer = m_cameraSwitchTime;
                m_camerasInTransition = false;
            }
        }
    }

    protected abstract void objectControl();

    protected virtual void timerEndActions() {
        
    }

    protected virtual void transitionInActions() {
        SwitchCameras();
        SwitchControls();
        
        ToggleInspectMode(true);


    }

    protected virtual void transitionOutActions() {

        SwitchCameras();
        SwitchControls();
        
        ToggleInspectMode(false);

    }

    protected abstract void EndingClickActions();

    protected virtual void SwitchCameras()
    {

        m_camerasInTransition = true;

        //switcha la camera

        
        m_mainCamera.enabled = !m_mainCamera.enabled;
        m_objectCamera.enabled = !m_objectCamera.enabled;       

    }



    protected virtual void SwitchControls()
    {
        //switcha i controlli
        m_userControl.enabled = !m_userControl.enabled;
        m_player.SetActive(!m_player.activeSelf);
    }

    public override bool loadData(int t_key, ulong t_data)
    {

        return base.loadData(t_key, t_data);
    }
}
