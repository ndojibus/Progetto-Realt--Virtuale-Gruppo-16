using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLaser : InteractableObject_Abstract {

	LaserBehaviour m_laser;
    GameObject m_item;

    private void Awake()
    {

        base.Awake();

        m_laser = GetComponentInChildren<LaserBehaviour>();
        if (m_laser == null)
            Debug.LogError(this.name + ": " + "Impossible to find LaserBehaviour!");

        m_item = this.transform.Find("LaserStart").Find("Rubino").gameObject;
        if (m_item == null)
            Debug.LogError(this.name + ": " + "Select an item!");
    }

    // Use this for initialization
    protected void Start () {
		base.Start ();
      
        //m_item.SetActive(true);
        m_equiped = false;
        m_inspectMode = false;

        createData(0);      //index 0, inizializzato a 0 perché non c'è il rubino
	}
	
	// Update is called once per frame
	protected void Update () {

        if (m_inspectMode == false && m_equiped == true && m_equiped != m_laser.enabled && !m_camerasInTransition)
        {

            EnableLaser();
            ChangeTransitor(1);

        }


        base.Update ();



        if (m_inspectMode && !m_equiped && Input.GetKeyDown(KeyCode.E) && m_inventory.HasRuby() && !m_camerasInTransition)
        {

            InsertRuby();
            UpdateUI();

        }
        
    }

    private void EnableLaser()
    {
        m_item.SetActive(m_equiped);
        m_laser.enabled = m_equiped;
        m_laser.transform.gameObject.SetActive(true);

        
    }

    private void InsertRuby()
    {
        //preso dall'inventario
        m_inventory.UseRuby();


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
                m_actionText = "Premi E per Ruotare la base";
                             
            }

        }

        base.UpdateUI();
        
        //***INSPECT MODE***
        if (m_inspectMode)
        {
            
            if (m_equiped && !m_laser.enabled)
            {
                m_uiManager.ToggleActionPanel(false);
                m_uiManager.ToggleDescriptionPanel(false);
            }
            else if (!m_equiped && m_inventory.HasRuby())
            {
                m_uiManager.ToggleActionPanel(true, "Premi E per Inserire");

            }
            else if (m_equiped)
            {
                m_uiManager.ToggleActionPanel(true, "Premi A e D per Ruotare");
                m_uiManager.ToggleDescriptionPanel(false);

            }
            else
            {
                m_uiManager.ToggleActionPanel(false);
            }

        }
        
        

        
       
       


    }


    

    

    protected override void TransitionOutActions()
    {
        base.TransitionOutActions();
    }



    protected override void SwitchControls()
	{
		base.SwitchControls ();
		m_laser.controlled = !m_laser.controlled;
	}

    public override bool loadData(int t_key, ulong t_data)
    {
        bool find = base.loadData(t_key, t_data);
        if (t_data == 1 && find)
        {
            m_equiped = true;
        }
        return find;
    }
}
