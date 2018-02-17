using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;

public abstract class InteractableObject_Abstract : PersistentData
{
    [SerializeField]
    protected string m_description;               //il testo descrittivo sulla GUI

    [SerializeField]
    protected string m_actionText;               //il testo dell'azione della GUIs



    [SerializeField]
    protected float m_cameraSwitchTime;      //quanto deve rimanere in transizione la camera prima di switchare al ritorno

    [SerializeField]
    protected int m_transitorID = 0;              //l'ID del cameratransitor a cui fa riferimento questo oggetto

    protected bool m_camerasInTransition = false;
    protected Camera m_mainCamera;
    protected FreeLookCam m_cameraControls;
    protected Camera m_objectCamera;
    protected CameraTransitor[] m_transitors;
    protected CameraTransitor m_transitor;
    protected ThirdPersonUserControl m_userControl;
    protected GameObject m_player;
    protected Canvas m_hudCanvas;
    protected UIManager m_uiManager;
    protected GameObject m_inventoryPanel;
    protected GameObject m_uiPosition;
    
    

    protected PlayerInventory m_inventory;

    protected bool m_inspectMode;
    protected bool m_equiped;
    protected bool m_finishedTransition = false;

    private bool m_playerInCollision;
    
    


    float m_timer;

    protected void Awake()
    {
        base.Awake();
        //aggiunto io test
        GameObject canvas = GameObject.Find("HUDCanvas");
        
        if (canvas != null)
        {
            m_uiManager = canvas.GetComponentInChildren<UIManager>();

            if (m_uiManager == null)
            {
                Debug.LogError(this.name + ": " + "Impossible to find UI Manager!");

            }
            
            m_inventory = canvas.GetComponentInChildren<PlayerInventory>();
            if (m_inventory == null)
                Debug.LogError(this.name + ": " + "Impossible to find a PlayerInventory!");
        }
        else
            Debug.LogError(this.name + ": " + "Impossible to find a canvas!");

        m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (m_mainCamera == null)
            Debug.LogError(this.name + ": " + "Impossible to find MainCamera!");

        m_cameraControls = m_mainCamera.GetComponentInParent<FreeLookCam>();
        if (m_cameraControls == null)
            Debug.LogError(this.name + ": " + "Impossible to find FreeLookCam!");

        m_objectCamera = GetComponentInChildren<Camera>();
        if (m_objectCamera == null)
            Debug.LogError(this.name + ": " + "Impossible to find object camera!");

        m_player = GameObject.FindGameObjectWithTag("Player");
        if (m_player != null)
        {
            m_userControl = m_player.GetComponent<ThirdPersonUserControl>();
            if (m_userControl == null)
                Debug.LogError(this.name + ": " + "Impossible to find ThirdPersonUserControl!");

        }
        else
            Debug.LogError(this.name + ": " + "Impossible to find a player!");

        

        m_transitors = GetComponentsInChildren<CameraTransitor>();
        if (m_transitors.Length > 0)
        {
            
        }
        else
            Debug.LogError(this.name + ": " + "Impossible to find any transitor attached to this object children!");




        m_inspectMode = false;


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

        m_uiPosition = this.transform.Find("UIPosition").gameObject;
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !m_inspectMode)
        {

            if ((Vector3.Angle(other.transform.forward, this.transform.position - other.transform.position) < 90f))
                m_playerInCollision = true;
            else
            {
                m_playerInCollision = false;
            }
            UpdateUI();
           
        }
        


        //Se c'è collisione con il player e viene premuto e si passa in inspect mode
        if (m_playerInCollision && Input.GetKeyDown(KeyCode.E))
        {
            TransitionInActions();
            m_transitor.forward = !m_transitor.forward;

            m_inspectMode = true;
            UpdateUI();



        }
        
    }

    //Da Cambiare 
    protected void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            m_playerInCollision = false;
            UpdateUI();

            
        }
        

    }

    //Attiva/disattiva il inspect mode, cioè pannello descrizione e pannello continue
    protected void ToggleInspectMode(bool active)
    {
        
        

        m_inspectMode = active;

    }

    

    // Update is called once per frame
    protected void Update()
    {

        if (m_inspectMode && Input.GetKeyDown(KeyCode.Escape))
        {
            TransitionOutActions();
            
            m_transitor.forward = !m_transitor.forward;

            UpdateUI();

            

        }

        if (m_camerasInTransition)
        {
            UpdateUI();

            m_timer -= Time.deltaTime;

            if (m_timer <= 0f)
            {

                m_timer = m_cameraSwitchTime;
                m_camerasInTransition = false;
                m_player.SetActive(!m_player.activeSelf);

                //switcha la camera
                if (!m_inspectMode)
                {
                    m_mainCamera.enabled = !m_mainCamera.enabled;
                    m_cameraControls.enabled = !m_cameraControls.enabled;
                    m_objectCamera.enabled = !m_objectCamera.enabled;
                }

                UpdateUI();
               

                //appena finito transizione
            }
        }
    }

   

    protected virtual void TransitionInActions() {
        m_inspectMode = true;
        SwitchCameras();
        SwitchControls();

        
        //ToggleInspectMode(true);


    }

    protected virtual void TransitionOutActions() {

        m_inspectMode = false;
        SwitchCameras();
        SwitchControls();

        
        //ToggleInspectMode(false);

    }

    protected virtual void SwitchCameras()
    {
        
        m_camerasInTransition = true;
        if (m_inspectMode)
        {
            m_mainCamera.enabled = !m_mainCamera.enabled;
            m_cameraControls.enabled = !m_cameraControls.enabled;
            m_objectCamera.enabled = !m_objectCamera.enabled;
        }



    }

    protected virtual void SwitchControls()
    {
        //switcha i controlli
        m_userControl.canMove = !m_userControl.canMove;
        //m_player.SetActive(!m_player.activeSelf);
        
    }

    protected virtual void ChangeTransitor(int transitorID)
    {

        
        m_transitors[m_transitorID].enabled = false;
        m_transitorID = transitorID;
        m_transitors[m_transitorID].enabled = true;
        //m_transitors[m_transitorID].forward = false;
        m_transitor = m_transitors[m_transitorID];
    }

    protected virtual void UpdateUI()
    {

        if (m_camerasInTransition)
        {
            m_uiManager.ToggleActionPanel(false);
            m_uiManager.ToggleInspectModeUI(false, m_description);

        }
        else
        {
            if (m_playerInCollision)
            {

                m_uiManager.ToggleActionPanel(true, m_actionText, m_uiPosition.transform);
            }
            else
            {
                m_uiManager.ToggleActionPanel(false);
            }


            m_uiManager.ToggleInspectModeUI(m_inspectMode, m_description);


        }


        
        

    }

    public override bool loadData(int t_key, ulong t_data)
    {

        return base.loadData(t_key, t_data);
    }
}
