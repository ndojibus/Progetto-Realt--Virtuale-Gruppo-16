using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInteraction : PersistentData
{
    List<GameObject> m_childs;
    CanvasGroup m_actionCanvas;
    int m_activeIndex = -1;         //persistent data, index 0
    bool m_activated = false;

	// Use this for initialization
	void Start () {
        m_childs = new List<GameObject>();
        foreach (Transform child in transform)
            m_childs.Add(child.gameObject);
        if (m_childs.Count == 0)
            Debug.LogError("Impossible to find chidren!");
        m_actionCanvas = GameObject.Find("ActionCanvas").GetComponent<CanvasGroup>();
        if (m_actionCanvas == null)
            Debug.LogError("Impossible to find ActionCanvas!");

        createData((UInt64)m_activeIndex);      //so index is 0
    }

    // Update is called once per frame
    void OnTriggerStay(Collider other) {
        if ((other.tag == "Player") && (Vector3.Angle(other.transform.forward, this.transform.position - other.transform.position) < 90f))
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
        if (Input.GetButtonDown("Fire1") && m_activated)
        {
            if (m_activeIndex < 0)
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
            SceneControl.sceneControl.Save();
        }
    }

    public override bool loadData(int t_key, UInt64 t_data) {
        bool find = base.loadData(t_key, t_data);
        if (((int)t_data >= 0))
        {
            m_activeIndex = (int)t_data;
            m_childs[m_activeIndex].SetActive(true);
        }
        return find;
    }
}
