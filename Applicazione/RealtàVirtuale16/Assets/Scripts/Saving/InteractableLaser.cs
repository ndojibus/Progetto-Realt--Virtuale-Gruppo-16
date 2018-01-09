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
            Debug.LogError("Impossible to find LaserBehaviour!");

        m_item = this.transform.Find("LaserStart").Find("Rubino").gameObject;
        if (m_item == null)
            Debug.LogError("Select an item!");
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
		base.Update ();

        if (m_equiped != m_item.activeSelf)
        {
            m_item.SetActive(m_equiped);
            m_laser.enabled = m_equiped;

            m_transitors[m_transitorID].enabled = false;
            m_transitorID = 1;
            m_transitors[m_transitorID].enabled = true;
            m_transitors[m_transitorID].forward = false;
            m_transitor = m_transitors[m_transitorID];
        }

        if (m_inspectMode && !m_equiped && Input.GetKeyDown(KeyCode.E) && m_inventory.HasRuby() && !m_camerasInTransition)
        {


            InsertRuby();
            //SetAction();

        }
        
    }


    private void InsertRuby()
    {
        //preso dall'inventario
        m_inventory.UseRuby();


        m_item.SetActive(true);

        m_equiped = true;

        toggleInspectPanel(false);
        toggleDescriptionPanel(false);

        saveData(0, 1);     //inserisci il rubino, index 0 diventa 1

        
    }


    protected override void objectControl() {
	}

    protected override void SetAction() 
    {
        if(!m_equiped && !m_inspectMode)
        {

            m_inspectText.text = "Premi E per Esaminare";
            toggleInspectPanel(true);
        }else if (m_equiped && !m_inspectMode)
        {
            m_inspectText.text = "Premi E per Ruotare la Base";
            toggleInspectPanel(true);

        }else if (m_inspectMode && m_inventory.HasRuby())
        {
            m_inspectText.text = "Premi E per Inserire";
            toggleInspectPanel(true);

        }
        else if (m_equiped && m_inspectMode)
        {
            m_inspectText.text = "Premi A e D per Ruotare";
            toggleDescriptionPanel(false);
            toggleInspectPanel(true);

        }
        else 
        {
            toggleInspectPanel(false);
        }
    }


    protected override void transitionOutActions()
    {
        base.transitionOutActions();
        if (m_equiped != m_laser.enabled && m_equiped == true)
        {
            m_laser.enabled = m_equiped;

            m_transitors[m_transitorID].enabled = false;
            m_transitorID = 1;
            m_transitors[m_transitorID].enabled = true;
            m_transitor = m_transitors[m_transitorID];
        }
    }

    protected override void EndingClickActions() {

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
