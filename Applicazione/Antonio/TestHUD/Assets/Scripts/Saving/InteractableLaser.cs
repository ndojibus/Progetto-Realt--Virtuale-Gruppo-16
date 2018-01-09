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
    }

    // Use this for initialization
    protected void Start () {
		base.Start ();


        
        //m_item.SetActive(true);
        equiped = false;
        inspectMode = false;
	}
	
	// Update is called once per frame
	protected void Update () {
		base.Update ();

        //poi ti spiego
        if (equiped != m_laser.enabled)
        {

            m_laser.enabled = equiped;
        }
        if (inspectMode && !equiped && Input.GetKeyDown(KeyCode.E) && !m_camerasInTransition)
        {


            InsertRuby();
            SetAction();

        }
        
    }


    //non ha senso quello che ho scritto, o meglio ti spiego a voce
    private void InsertRuby()
    {
        //preso dall'inventario
        m_inventory.UseRuby();


        m_item.SetActive(true);

        equiped = true;

        
        transitionOutActions();
        transitors[m_transitorID].enabled= false;
        m_transitorID = 1;
        transitors[m_transitorID].enabled = true;
        m_transitor = transitors[m_transitorID];


    }

    protected override void setActionText() {

        //m_actionText.text = "RUOTA";

        inspectText.text = "RUOTA";

    }

    protected override void objectControl() {


		if (!m_laser.enabled)
			m_laser.enabled = true;

		if (m_laser.controlled) {
			m_actionText.text = "RITORNA";
			m_actionCanvas.alpha = 1;
			m_activated = true;
		}
	}

    protected override void SetAction() 
    {
        if(!equiped && !inspectMode)
        {

            inspectText.text = "Premi E per Esaminare";
            toggleInspectPanel(true);
        }else if (equiped && !inspectMode)
        {
            inspectText.text = "Premi E per Ruotare la Base";
            toggleInspectPanel(true);

        }else if (inspectMode && m_inventory.HasRuby())
        {
            inspectText.text = "Premi E per Inserire";
            toggleInspectPanel(true);

        }
        else if (equiped && inspectMode)
        {
            inspectText.text = "Premi A e D per Ruotare";
            toggleDescriptionPanel(false);
            toggleInspectPanel(true);

        }
        else 
        {
            toggleInspectPanel(false);
        }
    }

    

    protected override void EndingClickActions() {

	}

	protected override void SwitchControls()
	{
		base.SwitchControls ();
		m_laser.controlled = !m_laser.controlled;
	}
}
