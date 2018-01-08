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
    protected string m_action;                    //il testo che indica al giocatore cosa fare nella GUI

    [SerializeField]
    protected float m_cameraSwitchTime = 4f;      //quanto deve rimanere in transizione la camera prima di switchare al ritorno

    [SerializeField]
    protected int m_transitorID = 0;              //l'ID del cameratransitor a cui fa riferimento questo oggetto

    protected CanvasGroup m_actionCanvas;         
    protected Text m_actionText;
    protected Text m_descriptionText;
    protected bool m_activated = false;
    protected bool m_camerasInTransition = false;
    protected bool m_actionDone = false;
    protected Camera m_mainCamera;
    protected Camera m_objectCamera;
    protected CameraTransitor[] transitors;
    protected CameraTransitor m_transitor;
    protected ThirdPersonUserControl m_userControl;
    protected GameObject m_player;
    protected Canvas hudCanvas;
    protected GameObject inventoryPanel;
    protected GameObject inspectPanel;
    protected Text inspectText;
    protected GameObject descriptionPanel;
    protected Text descriptionText;
    protected GameObject continuePanel;
    protected Text continueText;

    protected PlayerInventory m_inventory;

    protected bool inspectMode;
    protected bool equiped;
    


    float m_timer;

    protected void Awake()
    {
        //aggiunto io test
        GameObject canvas = GameObject.Find("HUDCanvas");
        if (canvas != null)
        {

            inspectPanel = canvas.transform.Find("InspectPanel").gameObject;
            inspectText = inspectPanel.GetComponentInChildren<Text>();
            if (inspectPanel == null || inspectText== null)
                Debug.LogError("Impossible to find inspect!");
            else
            {
                inspectPanel.SetActive(false);

            }

            descriptionPanel = canvas.transform.Find("DescriptionPanel").gameObject;
            descriptionText = descriptionPanel.GetComponentInChildren<Text>();
            if (descriptionPanel == null || descriptionText == null)
                Debug.LogError("Impossible to find description!");
            else
            {
                descriptionPanel.SetActive(false);

            }

            continuePanel = canvas.transform.Find("ContinuePanel").gameObject;
            continueText = continuePanel.GetComponentInChildren<Text>();
            if (continuePanel == null || continueText == null)
                Debug.LogError("Impossible to find continue!");
            else
            {
                continuePanel.SetActive(false);

            }
        }
        else
            Debug.LogError("Impossible to find a canvas!");

        GameObject sceneManager = GameObject.Find("SceneManager");
        if (sceneManager != null)
        {
            m_inventory = sceneManager.GetComponent<PlayerInventory>();
            if (m_inventory == null)
                Debug.LogError("Impossible to find a PlayerInventory!");
        }
        else
            Debug.LogError("Impossible to find a scene manager!");

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

        inspectMode = false;

        transitors = GetComponentsInChildren<CameraTransitor>();
        if (transitors.Length > 0)
        {
            
        }
        else
            Debug.LogError("Impossible to find any transitor attached to this object children!");



    }

    // Use this for initialization
    protected void Start () {
        m_timer = m_cameraSwitchTime;

        int i;
        for (i = 0; i < transitors.Length; i++)
        {

            if (transitors[i].transitorID == m_transitorID)
                break;
        }
        if (i < transitors.Length)
            m_transitor = transitors[i];
        else
            Debug.LogError("Impossible to find camera transitor!");



        GameObject canvas = GameObject.Find("ActionCanvas");
        if (canvas != null)
        {
            Text[] provaText = canvas.GetComponentsInChildren<Text>();
            m_actionText = provaText[0];
            if (m_actionText == null)
                Debug.LogError("Impossible to find ActionText!");
            m_descriptionText = provaText[1];
            if (m_descriptionText == null)
                Debug.LogError("Impossible to find DescriptionText!");
            m_actionCanvas = canvas.GetComponent<CanvasGroup>();
            if (m_actionCanvas == null)
                Debug.LogError("Impossible to find ActionCanvas!");
        }
        else
            Debug.LogError("Impossible to find a canvas!");

        


        
    }

    protected void OnTriggerEnter(Collider other)
    {

        if(other.tag== "Player" && !inspectMode)
        {
            //toggleInspectPanel(true);
            //setActionText();

            SetAction();

        }
    }

    protected void OnTriggerStay(Collider other)
    {

        //Se c'è collisione con il player e viene premuto e si passa in inspect mode
        if (other.tag == "Player" && !inspectMode && Input.GetKeyDown(KeyCode.E))
        {

            
            transitionInActions();

            m_transitor.forward = !m_transitor.forward;
            SetAction();

            //m_actionCanvas.alpha = 0;
            //m_activated = false;

            //EndingClickActions();
            //setActionText();


        }

       

        /*
        if ((other.tag == "Player")
            && (Vector3.Angle(other.transform.forward, this.transform.position - other.transform.position) < 90f)
            && !m_actionDone)
        {

            
            
            setActionText();
            //m_actionCanvas.alpha = 1;
            //m_activated = true;
        }
        else
        {
            
            //m_actionCanvas.alpha = 0;
            //m_activated = false;
        }

        */
    }


    //Da Cambiare 
    protected void OnTriggerExit(Collider other)
    {

        toggleInspectPanel(false);
        
    }

    //Attiva/disattiva il pannello Inspect
    protected void toggleInspectPanel(bool active)
    {

        if(inspectPanel.activeSelf!= active)
        {
            inspectPanel.SetActive(!inspectPanel.activeSelf);
            m_activated = inspectPanel.activeSelf;

        }

    }

    //Attiva/disattiva il inspect mode, cioè pannello descrizione e pannello continue
    protected void ToggleInspectMode(bool active)
    {

        

        if(continuePanel.activeSelf != active)
        {
            continuePanel.SetActive(!continuePanel.activeSelf);
  
        }

        if (descriptionPanel.activeSelf != active)
        {
            descriptionText.text = m_description;
            descriptionPanel.SetActive(!descriptionPanel.activeSelf);
            

        }

        inspectMode = active;

    }

    protected abstract void SetAction();

    // Update is called once per frame
    protected void Update()
    {

        if (inspectMode && Input.GetKeyDown(KeyCode.Escape))
        {
            
            
            transitionOutActions();

            m_transitor.forward = !m_transitor.forward;
            SetAction();


            //m_actionCanvas.alpha = 0;
            //m_activated = false;
            //endingClickActions();
            //setActionText();
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


        /*
        if (!m_actionDone)
        {
            if (!m_transitor.enabled)
                m_transitor.enabled = true;

            

            objectControl();

            if (m_camerasInTransition)
            {
                m_descriptionText.text = "";
                m_actionText.text = "";

                m_timer -= Time.deltaTime;
                if (m_timer <= 0f)
                {
                    timerEndActions();

                    m_timer = m_cameraSwitchTime;
                    m_camerasInTransition = false;
                }
            }

        }


        
        else if (Input.GetButtonDown("Fire1") && m_activated)
        {
            if (m_mainCamera.enabled)
            {
                transitionInActions();
            }
            else
            {
                transitionOutActions();
            }

            m_transitor.forward = !m_transitor.forward;


            m_actionCanvas.alpha = 0;
            m_activated = false;

            endingClickActions();
        }*/





    }

    protected abstract void setActionText();
    protected abstract void objectControl();

    protected virtual void timerEndActions() {
        
    }

    protected virtual void transitionInActions() {
        SwitchCameras();
        SwitchControls();
        
        ToggleInspectMode(true);
        Debug.Log("Inspect mode active " + inspectMode);


    }

    protected virtual void transitionOutActions() {

        SwitchCameras();
        SwitchControls();
        
        ToggleInspectMode(false);
        Debug.Log("Inspect mode active " + inspectMode);

    }

    protected abstract void EndingClickActions();

    protected virtual void SwitchCameras()
    {
        
        m_camerasInTransition = true;
        
        Debug.Log("Camera switching");
        //switcha la camera

        
        m_mainCamera.enabled = !m_mainCamera.enabled;
        m_objectCamera.enabled = !m_objectCamera.enabled;

        

    }
    





    protected virtual void SwitchControls()
    {
        Debug.Log("Control switching");
        //switcha i controlli
        m_userControl.enabled = !m_userControl.enabled;
        m_player.SetActive(!m_player.activeSelf);
    }

    public override bool loadData(int t_key, ulong t_data)
    {

        return base.loadData(t_key, t_data);
    }
}
