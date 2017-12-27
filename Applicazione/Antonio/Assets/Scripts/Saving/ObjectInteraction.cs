using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : PersistentData
{
    [SerializeField]
    UInt64 m_objectID = 0;

    GameObject m_child;
    CanvasGroup m_actionCanvas;
    PlayerInventory m_inventory;
    bool m_picked = false;
    float m_timer;
    

    // Use this for initialization
    void Start()
    {
        m_child = transform.GetChild(0).gameObject;
        if (m_child == null)
            Debug.LogError("Impossible to find child!");

        GameObject canvas = GameObject.Find("ActionCanvas");
        if (canvas != null)
        {
            m_actionCanvas = canvas.GetComponent<CanvasGroup>();
            if (m_actionCanvas == null)
                Debug.LogError("Impossible to find ActionCanvas!");
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


        createData(0);      //so index is 0
    }

    // Update is called once per frame
    void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Player")
            && (Vector3.Angle(other.transform.forward, this.transform.position - other.transform.position) < 90f)
            && !m_picked
            )
        {
            m_actionCanvas.alpha = 1;
        }
        else
        {
            m_actionCanvas.alpha = 0;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !m_picked) {
                m_picked  = true;
                m_child.SetActive(false);

                m_inventory.addItem(m_objectID);
                saveData(0, 1);
        }
            
            
            //SceneControl.sceneControl.Save();

        }

    public override bool loadData(int t_key, UInt64 t_data)
    {
        bool find = base.loadData(t_key, t_data);
        if (((int)t_data >= 0) && find)
        {
            m_picked = (t_data == 0) ? false : true;
            if (m_picked)
                m_child.SetActive(false);
        }
        return find;
    }
}
