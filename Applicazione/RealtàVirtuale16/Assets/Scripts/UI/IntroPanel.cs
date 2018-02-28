using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPanel : MonoBehaviour {

    private CanvasGroup m_cg;

    private bool isActive;

    private void Awake()
    {
        m_cg = this.GetComponent<CanvasGroup>();
    }

    // Use this for initialization
    void Start () {

        DisableIntro();
		
	}

    void Update()
    {

        if(isActive && Input.GetKeyDown(KeyCode.Escape))
        {
            DisableIntro();
        }

    }

    public void ActiveIntro()
    {
        m_cg.alpha = 1;

        m_cg.interactable = true;
        m_cg.blocksRaycasts = true;

        isActive = true;

        Time.timeScale = 0f;

    }

    public void DisableIntro()
    {
        m_cg.alpha = 0;
        m_cg.interactable = false;
        m_cg.blocksRaycasts = false;

        isActive = false;

        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    
}
