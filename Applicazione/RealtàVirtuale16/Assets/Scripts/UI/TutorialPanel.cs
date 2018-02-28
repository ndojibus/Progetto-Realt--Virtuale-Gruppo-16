using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour {

    private CanvasGroup m_cg;

	// Use this for initialization
	void Awake () {

        m_cg = this.GetComponent<CanvasGroup>();
		
	}

    private void Start()
    {
        DisableTutorial();
    }


    public void ActiveTutorial()
    {
        m_cg.alpha = 1;
       
        m_cg.interactable = true;
        m_cg.blocksRaycasts = true;

        Time.timeScale = 0f;

    }

    public void DisableTutorial()
    {
        m_cg.alpha = 0;
        m_cg.interactable = false;
        m_cg.blocksRaycasts = false;

        Time.timeScale = 1.0f;
    }
}
