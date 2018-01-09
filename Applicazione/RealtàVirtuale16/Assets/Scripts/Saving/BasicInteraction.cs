using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class BasicInteraction : PersistentData
{
    [SerializeField]
    GameObject m_item;

    [SerializeField]
    string m_description;

    [SerializeField]
    string m_action;

    [SerializeField]
    float m_timerTime = 1f;

    [SerializeField]
    UInt64 m_laserHeadID = 0;

    [SerializeField]
    bool m_hasHead = false;

    //List<GameObject> m_childs;
    CanvasGroup m_actionCanvas;
    Text m_actionText;
    Text m_descriptionText;
    bool m_activated = false;
    bool m_examining = false;
    bool m_camerasInTransition = false;
    bool m_playerHasHead;
    LaserBehaviour m_laser;
    PlayerInventory m_inventory;
    Camera m_mainCamera;
    Camera m_laserCamera;
    CameraTransitor[] m_transitor;
    int m_transitorIndex = 0;
    ThirdPersonUserControl m_userControl;
    GameObject m_player;

    float m_timer;

    // Use this for initialization
    void Start () {
        //m_childs = new List<GameObject>();
        //foreach (Transform child in transform)
        //    m_childs.Add(child.gameObject);
        //if (m_childs.Count == 0)
        //    Debug.LogError("Impossible to find chidren!");

        m_timer = m_timerTime;

        m_laser = GetComponentInChildren<LaserBehaviour>();
        if (m_laser == null)
            Debug.LogError("Impossible to find LaserBehaviour!");

        m_laserCamera = GetComponentInChildren<Camera>();
        if (m_laserCamera == null)
            Debug.LogError("Impossible to find laser camera!");

        m_transitor = GetComponentsInChildren<CameraTransitor>();
            if (m_transitor == null)
                Debug.LogError("Impossible to find transitor!");


        m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (m_mainCamera == null)
            Debug.LogError("Impossible to find MainCamera!");

        GameObject canvas = GameObject.Find("ActionCanvas");
        if (canvas != null) {
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

        GameObject sceneManager = GameObject.Find("SceneManager");
        if (sceneManager != null)
        {
            m_inventory = sceneManager.GetComponent<PlayerInventory>();
            if (m_inventory == null)
                Debug.LogError("Impossible to find a PlayerInventory!");
        }
        else
            Debug.LogError("Impossible to find a scene manager!");

        createData((UInt64)transform.localRotation.eulerAngles.y);      //so index is 0
    }

    // Update is called once per frame
    void OnTriggerStay(Collider other) {
        

        if ((other.tag == "Player") 
            && (Vector3.Angle(other.transform.forward, this.transform.position - other.transform.position) < 90f))
        {
            if (!m_hasHead)
                m_actionText.text = "ESAMINA";
            else
                m_actionText.text = "RUOTA";

            m_actionCanvas.alpha = 1;
            m_activated = true;
        }
        else
        {
            m_actionCanvas.alpha = 0;
            m_activated = false;
        }
    }

    void Update() {
        if (m_examining) {
            m_playerHasHead = m_inventory.findItem(m_laserHeadID);
            if (!m_playerHasHead)
                m_actionText.text = "CONTINUA";
            else
                m_actionText.text = m_action;
            m_descriptionText.text = m_description;

            m_actionCanvas.alpha = 1;
            m_activated = true;

            m_examining = false;
        }

        if (m_hasHead && !m_laser.enabled)
            m_laser.enabled = true;

        if (m_hasHead && m_laser.controlled) {
            m_actionText.text = "CONTINUA";
            m_actionCanvas.alpha = 1;
            m_activated = true;
        }

        if (m_camerasInTransition)
        {
            m_descriptionText.text = "";
            m_actionText.text = "";

            m_timer -= Time.deltaTime;
            if (m_timer <= 0f)
            {
                
                switchCameras();
                m_timer = m_timerTime;
                m_camerasInTransition = false;
            }

        }
        else if (Input.GetButtonDown("Fire1") && m_activated )
        {
            if (m_mainCamera.enabled)
            {
                switchCameras();
                switchControls(m_hasHead);
            }
            else
            {
                switchControls(m_hasHead);
                m_camerasInTransition = true;
            }

            m_transitor[m_transitorIndex].forward = !m_transitor[m_transitorIndex].forward;
            

            m_actionCanvas.alpha = 0;
            m_activated = false;

            if (!m_hasHead && m_playerHasHead)
            {

                m_transitor[m_transitorIndex++].enabled = false;
                m_transitor[m_transitorIndex].enabled = true;

                m_hasHead = true;
                m_inventory.deleteItem(m_laserHeadID);

                m_item.SetActive(!m_item.activeSelf);
            }
            else if (!m_hasHead && !m_examining)
                m_examining = true;
        }
    }

    public override bool loadData(int t_key, UInt64 t_data) {
        bool find = base.loadData(t_key, t_data);
        if (((int)t_data >= 0) && find)
        {
            transform.localRotation.eulerAngles.Set(0, (float)t_data, 0);
            //m_childs[m_activeIndex].SetActive(true);
        }
        return find;
    }

    private void switchCameras()
    {
        Debug.Log("Camera switching");
        //switcha la camera
        m_mainCamera.enabled = !m_mainCamera.enabled;
        m_laserCamera.enabled = !m_laserCamera.enabled;
    }

    private void switchControls(bool t_laser)
    {
        Debug.Log("Control switching");
        //switcha i controlli
        m_userControl.enabled = !m_userControl.enabled;
        if (t_laser)
            m_laser.controlled = !m_laser.controlled;
        m_player.SetActive(!m_player.activeSelf);
    }
}
