using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrap : InteractableObject_Abstract
{
    CameraTransitor m_trapTransitor;
    GameObject m_item;

    private void Awake()
    {
        base.Awake();

        m_item = this.transform.Find("Hank").gameObject;
        if (m_item == null)
            Debug.LogError(this.name + ": " + "Select an item!");
    }

    // Use this for initialization
    void Start()
    {
        base.Start();

        int i = 0;
        for (i = 0; i < m_transitors.Length; i++)
        {

            if (m_transitors[i].transitorID == 100) //valore standard per il transitor della botola
                break;
        }
        if (i < m_transitors.Length)
            m_trapTransitor = m_transitors[i];
        else
            Debug.LogError(this.name + ": " + "Can't find a trap transitor!");

        m_equiped = false;
        m_inspectMode = false;

        createData(1);  //index 0, 1 significa che il rubino è ancora incastonato
    }

    // Update is called once per frame
    void Update()
    {

        base.Update();



        if (m_inspectMode && !m_equiped && Input.GetKeyDown(KeyCode.E) && m_inventory.HasKey() && !m_camerasInTransition)
        {

            InsertRuby();
            UpdateUI();

        }
    }

    private void InsertRuby()
    {
        //preso dall'inventario
        m_inventory.UseKey();


        m_item.SetActive(true);

        m_equiped = true;


        saveData(0, 1);     //inserisci il rubino, index 0 diventa 1


    }



    protected override void UpdateUI()
    {

        //***NON INSPECT MODE***

        if (!m_inspectMode)
        {
            if (!m_equiped)
            {

                m_actionText = "Premi E per Esaminare";

            }
            else
            {
                m_actionText = "";

            }

        }

        base.UpdateUI();

        //***INSPECT MODE***

        if (!m_camerasInTransition)
        {
            if (m_inspectMode)
            {

                if (m_equiped)
                {
                    m_uiManager.ToggleActionPanel(false);
                    m_uiManager.ToggleDescriptionPanel(false);
                }
                else if (!m_equiped && m_inventory.HasKey())
                {
                    m_uiManager.ToggleActionPanel(true, "Premi E per Inserire");

                }
                else
                {
                    m_uiManager.ToggleActionPanel(false);
                }

            }

        }
        
    }

    protected override void TransitionOutActions()
    {
        base.TransitionOutActions();

        //attiva il transitor della botola
        if (m_equiped) {
            m_trapTransitor.forward = !m_trapTransitor.forward;
        }

    }


    public override bool loadData(int t_key, ulong t_data)
    {
        bool find = base.loadData(t_key, t_data);
        if (t_data == 0 && find)
        {
            //m_equiped = false;

        }
        return find;
    }
}
