﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    bool GamePaused = false;

    private GameObject m_inventoryPanel;
    private GameObject m_actionPanel;
    private GameObject m_continuePanel;
    private GameObject m_descriptionPanel;
    private GameObject m_pausePanel;

    private Text m_actionText;
    private Text m_continueText;
    private Text m_descriptionText;

    private Transform m_targetobject;

    private GameObject m_actionPanel1elem;
    private GameObject m_actionPanel2elem;
    private GameObject m_continuePanelV2;

    private Text m_continueTextV2;
    private Text m_actionTextNewPanel;
    private Text m_actionText1;
    private Text m_actionText2;

    private bool inspectmode;


    private void Awake()
    {
        /*OLD CODE VECCHI PANNELLI
         
        m_actionPanel = this.transform.Find("ActionPanel").gameObject;
        m_actionText = m_actionPanel.GetComponentInChildren<Text>();
        if (m_actionPanel == null)
        {
            Debug.Log(this.name + ": " + "Impossible to find ActionPanel");
        }


        m_continuePanel = this.transform.Find("ContinuePanel").gameObject;
        m_continueText = m_continuePanel.GetComponentInChildren<Text>();
        if (m_continuePanel == null || m_continueText== null)
        {
            Debug.Log(this.name + ": " + "Impossible to find continuePanel");
        }
         
         */

        m_inventoryPanel = this.transform.Find("InventoryPanel").gameObject;
        if (m_inventoryPanel == null)
        {
            Debug.Log(this.name + ": " +"Impossible to find Inventory Panel");
        }

        

        

        m_descriptionPanel = this.transform.Find("DescriptionPanel").gameObject;
        m_descriptionText = m_descriptionPanel.GetComponentInChildren<Text>();
        if (m_descriptionPanel == null || m_descriptionText== null)
        {
            Debug.Log(this.name + ": " + "Impossible to find descriptionPanel");
        }

        m_pausePanel = this.transform.Find("PausePanelBackground").gameObject;
        if (m_pausePanel == null)
        {
            Debug.Log(this.name + ": " + "Impossible to find pausePanel");
        }

        //nuovipannelli
        m_actionPanel1elem = this.transform.Find("ActionPanel1elem").gameObject;
        m_actionTextNewPanel = m_actionPanel1elem.GetComponentInChildren<Text>();
        if (m_actionPanel1elem == null)
        {
            Debug.Log(this.name + ": " + "Impossible to find ActionPanel1elem");
        }

        m_actionPanel2elem = this.transform.Find("ActionPanel2elem").gameObject;
        m_actionText1 = m_actionPanel2elem.GetComponentsInChildren<Text>()[0];
        m_actionText2 = m_actionPanel2elem.GetComponentsInChildren<Text>()[1];
        if (m_actionPanel2elem == null)
        {
            Debug.Log(this.name + ": " + "Impossible to find m_2elemActionPanel");
        }

        m_continuePanelV2 = this.transform.Find("ContinuePanelV2").gameObject;
        m_continueTextV2 = m_continuePanelV2.GetComponentInChildren<Text>();

        //poi ci metto i controlli







    }

    private void Start()
    {
        ToggleActionPanel(false);
        ToggleDescriptionPanel(false);
        ToggleContinuePanel(false);

        m_actionPanel1elem.SetActive(false);
        m_actionPanel2elem.SetActive(false);
        m_continuePanelV2.SetActive(false);

        ToggleInspectModeUI(false,"");

        m_actionText2.text = "Esc per Continuare";
        


    }




    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GamePaused)
                PauseGame();
            else
                ResumeGame();
        }

        /*
        if (m_actionPanel.activeSelf)
        {
            m_actionPanel.transform.position = Camera.main.WorldToScreenPoint(m_targetobject.position);
        }
        */
        if (m_actionPanel1elem.activeSelf)
        {
            m_actionPanel1elem.transform.position = Camera.main.WorldToScreenPoint(m_targetobject.position);
        }



    }

    public void PauseGame()
    {
        //Disabilito tutti i pannelli 
        m_inventoryPanel.SetActive(false);
        m_actionPanel.SetActive(false);
        m_continuePanel.SetActive(false);
        m_descriptionPanel.SetActive(false);

        //attivo il pause menu
        m_pausePanel.SetActive(true);

        //var
        Time.timeScale = 0f;
        GamePaused = true;

    }



    public void ResumeGame()
    {
        //disabilito il pause menu
        m_pausePanel.SetActive(false);

        //abilito solo il pannello dell'inventario mentre gli altri vedo 
        m_inventoryPanel.SetActive(true);
        


        //var
        Time.timeScale = 1f;
        GamePaused = false;


    }

    

    //setta il testo dell'Action Panel
    public void SetActionText(string text)
    {
        m_actionText.text = text;
    }
    public void SetDescriptionText(string text)
    {
        m_descriptionText.text = text;
    }
    public void SetContinueText(string text)
    {
        m_continueText.text = text;
    }

    //Attiva/disattiva il pannello Inspect
    public void ToggleActionPanel(bool active)
    {

        /*OLD CODE VECCHI PANNELLI
         
         
         if (m_actionPanel.activeSelf != active)
        {
            m_actionPanel.SetActive(!m_actionPanel.activeSelf);

        }
        */

        if (inspectmode)
        {


            m_actionPanel1elem.SetActive(false);
            ToggleContinuePanel(!active);
            //m_actionText2.text = actionText;

            if (m_actionPanel2elem.activeSelf != active)
            {
                m_actionPanel2elem.SetActive(!m_actionPanel2elem.activeSelf);

            }

        }
        else
        {

            //m_actionTextNewPanel.text = actionText;

            if (m_actionPanel1elem.activeSelf != active)
            {
                m_actionPanel1elem.SetActive(!m_actionPanel1elem.activeSelf);

            }

            if (m_actionPanel2elem.activeSelf != active)
            {
                m_actionPanel2elem.SetActive(!m_actionPanel2elem.activeSelf);

            }


        }




    }


    public void ToggleActionPanel(bool active, string actionText)
    {

        /*OLD CODE VECCHI PANNELLI
         m_actionText.text = actionText;
        if (m_actionPanel.activeSelf != active)
        {
            m_actionPanel.SetActive(!m_actionPanel.activeSelf);

        }
         
         */

        if (inspectmode)
        {


            m_actionPanel1elem.SetActive(false);
            ToggleContinuePanel(!active);
            m_actionText2.text = actionText;

            if (m_actionPanel2elem.activeSelf != active)
            {
                m_actionPanel2elem.SetActive(!m_actionPanel2elem.activeSelf);

            }

        }
        else
        {

            m_actionTextNewPanel.text = actionText;
                      
            if (m_actionPanel1elem.activeSelf != active)
            {
                m_actionPanel1elem.SetActive(!m_actionPanel1elem.activeSelf);

            }

            if (m_actionPanel2elem.activeSelf != active)
            {
                m_actionPanel2elem.SetActive(!m_actionPanel2elem.activeSelf);

            }


        }


        
        
        

        



    }


    //questo metodo attiva l'action panel che segue l'oggetto
    public void ToggleActionPanel(bool active, string actionText, Transform targetTransform)
    {
        /*  *********OLD CODE VECCHI PANNELLI************
        m_actionText.text = actionText;
        m_targetobject = targetTransform;
        if (m_actionPanel.activeSelf != active)
        {
            m_actionPanel.SetActive(!m_actionPanel.activeSelf);

        }
        */

        //se siamo in inspectMode dobbiamo disabilitare il primo pannello e mettere il secondo con due linee
        
            




        
        
            m_actionTextNewPanel.text = actionText;
            m_targetobject = targetTransform;

            if (m_actionPanel1elem.activeSelf != active)
            {
                m_actionPanel1elem.SetActive(!m_actionPanel1elem.activeSelf);

            }

        
        


    }

    public void ToggleDescriptionPanel(bool active)
    {

        if (m_descriptionPanel.activeSelf != active)
        {
           
            m_descriptionPanel.SetActive(!m_descriptionPanel.activeSelf);
        }

    }

    public void ToggleContinuePanel(bool active)
    {


        /* OLD CODE VECCHI PANNELLI
         if (m_continuePanel.activeSelf != active)
        {
            m_continuePanel.SetActive(!m_continuePanel.activeSelf);
        }
         
         
         */

        if (m_continuePanelV2.activeSelf != active)
        {
            m_continuePanelV2.SetActive(!m_continuePanelV2.activeSelf);
        }

        if (m_actionPanel2elem.activeSelf != active)
        {
            m_actionPanel2elem.SetActive(!m_actionPanel2elem.activeSelf);

        }







    }

    public void ToggleInspectModeUI(bool active, string descriptionText)
    {

        inspectmode = active;

        ToggleContinuePanel(active);

        

        if (m_descriptionPanel.activeSelf != active)
        {
            m_descriptionText.text = descriptionText;
            m_descriptionPanel.SetActive(!m_descriptionPanel.activeSelf);
            

        }

    }

    

    public void SaveGame()
    {

        SceneControl.sceneControl.Save();
    }

    public void LoadGame()
    {

        SceneControl.sceneControl.ReloadScene();
        ResumeGame();
    }







}
