using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class BasicInteraction : PersistentData
{
    [SerializeField]
    float m_timerTime = 1f;

    //List<GameObject> m_childs;
    CanvasGroup m_actionCanvas;
    int m_activeIndex = -1;         //persistent data, index 0
    bool m_activated = false;
    bool m_camerasInTransition = false;
    LaserBehaviour m_laser;
    Camera m_mainCamera;
    Camera m_laserCamera;
    CameraTransitor m_transitor;
    ThirdPersonUserControl m_userControl;

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


            m_mainCamera = GetComponentInChildren<Camera>();
            if (m_mainCamera == null)
                Debug.LogError("Impossible to find main camera!");

        m_laserCamera = GetComponentInChildren<Camera>();
        if (m_laserCamera == null)
            Debug.LogError("Impossible to find laser camera!");

        m_transitor = GetComponentInChildren<CameraTransitor>();
            if (m_transitor == null)
                Debug.LogError("Impossible to find transitor!");


        m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (m_mainCamera == null)
            Debug.LogError("Impossible to find MainCamera!");

        GameObject canvas = GameObject.Find("ActionCanvas");
        if (canvas != null) {
            m_actionCanvas = canvas.GetComponent<CanvasGroup>();
            if (m_actionCanvas == null)
                Debug.LogError("Impossible to find ActionCanvas!");
        }
        else
            Debug.LogError("Impossible to find a canvas!");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            m_userControl = player.GetComponent<ThirdPersonUserControl>();
            if (m_userControl == null)
                Debug.LogError("Impossible to find ThirdPersonUserControl!");

        }
        else
            Debug.LogError("Impossible to find a player!");

        createData((UInt64)transform.localRotation.eulerAngles.y);      //so index is 0
    }

    // Update is called once per frame
    void OnTriggerStay(Collider other) {
        if (((other.tag == "Player") 
            && (Vector3.Angle(other.transform.forward, this.transform.position - other.transform.position) < 90f))
            || m_laser.controlled)
        {
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

        if(m_camerasInTransition)
        {
            m_timer -= Time.deltaTime;
            if (m_timer <= 0f)
            {
                switchCameras();
                m_timer = m_timerTime; ;
                m_camerasInTransition = false;
            }

        }
        else
        if (Input.GetButtonDown("Fire1") && (m_activated || m_laser.controlled))
        {
            /*if (m_activeIndex < 0)
            {
                m_activeIndex = 0;
                m_childs[m_activeIndex].SetActive(true);
            }
            else
            {
                m_childs[m_activeIndex++].SetActive(false);
                if (m_activeIndex >= m_childs.Count)
                    m_activeIndex = -1;
                else
                    m_childs[m_activeIndex].SetActive(true);
            }
            saveData(0, (UInt64)m_activeIndex);
            SceneControl.sceneControl.Save();*/

            if (m_mainCamera.enabled)
            {
                switchCameras();
            }
            else
            {
                m_camerasInTransition = true;
            }

            m_transitor.forward = !m_transitor.forward;
            

            m_actionCanvas.alpha = 0;
            m_activated = false;
        }
    }

    public override bool loadData(int t_key, UInt64 t_data) {
        bool find = base.loadData(t_key, t_data);
        if (((int)t_data >= 0))
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

        //switcha i controlli
        m_userControl.enabled = !m_userControl.enabled;
        m_laser.controlled = !m_laser.controlled;
    }
}
