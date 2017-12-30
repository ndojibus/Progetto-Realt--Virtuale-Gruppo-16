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
    protected float m_cameraSwitchTime = 1f;      //quanto deve rimanere in transizione la camera prima di switchare al ritorno

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
    protected CameraTransitor m_transitor;
    protected ThirdPersonUserControl m_userControl;
    protected GameObject m_player;

    float m_timer;

    // Use this for initialization
    protected void Start () {
        m_timer = m_cameraSwitchTime;

        m_objectCamera = GetComponentInChildren<Camera>();
        if (m_objectCamera == null)
            Debug.LogError("Impossible to find laser camera!");

        CameraTransitor[] transitors = GetComponentsInChildren<CameraTransitor>();
        if (transitors.Length > 0) {
            int i;
            for (i = 0; i < transitors.Length; i++) {
                if (transitors[i].transitorID == m_transitorID)
                    break;
            }
            if (i < transitors.Length)
                m_transitor = transitors[i];
            else
                Debug.LogError("Impossible to find camera transitor!");
        }
        else
            Debug.LogError("Impossible to find any transitor attached to this object children!");


        m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (m_mainCamera == null)
            Debug.LogError("Impossible to find MainCamera!");

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

        m_player = GameObject.FindGameObjectWithTag("Player");
        if (m_player != null)
        {
            m_userControl = m_player.GetComponent<ThirdPersonUserControl>();
            if (m_userControl == null)
                Debug.LogError("Impossible to find ThirdPersonUserControl!");

        }
        else
            Debug.LogError("Impossible to find a player!");
    }

    protected void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Player")
            && (Vector3.Angle(other.transform.forward, this.transform.position - other.transform.position) < 90f)
            && !m_actionDone)
        {
            setActionText();

            m_actionCanvas.alpha = 1;
            m_activated = true;
        }
        else
        {
            m_actionCanvas.alpha = 0;
            m_activated = false;
        }
    }


    // Update is called once per frame
    protected void Update()
    {
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
            }
        }
    }

    protected abstract void setActionText();
    protected abstract void objectControl();

    protected virtual void timerEndActions() {
        switchCameras();
    }

    protected virtual void transitionInActions() {
        switchCameras();
        switchControls();
    }

    protected virtual void transitionOutActions() {
        switchControls();
        m_camerasInTransition = true;
    }

    protected abstract void endingClickActions();

    protected virtual void switchCameras()
    {
        Debug.Log("Camera switching");
        //switcha la camera
        m_mainCamera.enabled = !m_mainCamera.enabled;
        m_objectCamera.enabled = !m_objectCamera.enabled;
    }

    protected virtual void switchControls()
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
