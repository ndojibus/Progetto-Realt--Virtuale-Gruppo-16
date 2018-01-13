using System.Collections;
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
    

    private void Awake()
    {
        m_inventoryPanel = this.transform.Find("InventoryPanel").gameObject;
        if (m_inventoryPanel == null)
        {
            Debug.Log(this.name + ": " +"Impossible to find Inventory Panel");
        }

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
    }

    private void Start()
    {
        ToggleActionPanel(false);
        ToggleDescriptionPanel(false);
        ToggleContinuePanel(false);
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
       

        if (m_actionPanel.activeSelf != active)
        {
            m_actionPanel.SetActive(!m_actionPanel.activeSelf);

        }

    }

    public void ToggleActionPanel(bool active, string actionText)
    {
        m_actionText.text = actionText;
        if (m_actionPanel.activeSelf != active)
        {
            m_actionPanel.SetActive(!m_actionPanel.activeSelf);

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

        if (m_continuePanel.activeSelf != active)
        {
            m_continuePanel.SetActive(!m_continuePanel.activeSelf);
        }

    }

    public void ToggleInspectModeUI(bool active, string descriptionText)
    {



        if (m_continuePanel.activeSelf != active)
        {
            m_continuePanel.SetActive(!m_continuePanel.activeSelf);

        }

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
