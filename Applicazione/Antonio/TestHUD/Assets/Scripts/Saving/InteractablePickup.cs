using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePickup : InteractableObject_Abstract
{
    
    GameObject m_item;

    [SerializeField]
    UInt64 m_itemID = 0;

	[SerializeField]
	InteractableObject_Abstract m_nextBehaviour;

    [SerializeField]
    bool m_picking = false;

    
    bool m_playerHasObject = false;
    bool m_examining = false;

    // Use this for initialization
    void Start () {
        base.Start();

        descriptionText.text = m_description;

        m_item = this.transform.Find("Rubino").gameObject;
        

        if (m_item == null)
            Debug.LogError("Select an item!");

		if (m_nextBehaviour == null && !m_picking)
			Debug.LogError("Select the next behaviour!");

       

        equiped = true;
    }

    // Update is called once per frame
    void Update () {


        base.Update();


        if (inspectMode && equiped && Input.GetKeyDown(KeyCode.E) && !m_camerasInTransition)
        {
            
            TakeRuby();
            SetAction();

        }

        /*
        if (m_actionDone && !m_picking) {
			m_nextBehaviour.enabled = true;
			this.enabled = false;
		}
		*/
	}

    private void TakeRuby()
    {

        //inserito nell'inventario
        m_inventory.PickRuby();

        //cancellato dalla scena

        m_item.SetActive(false);
        equiped = false;


    }

    protected override void SetAction()
    {
        if (!inspectMode)
        {
            //m_actionText.text = "ESAMINA";

            inspectText.text = "Premi E per Esaminare";
            toggleInspectPanel(true);
        }
        else if (inspectMode && equiped)
        {
            inspectText.text = "Premi E per Raccogliere il Rubino";
            toggleInspectPanel(true);

        }
        else
        {
            toggleInspectPanel(false);

        }
    }


    // l'ho riscritta sopra
    protected override void setActionText() {

        if (!inspectMode)
        {
            //m_actionText.text = "ESAMINA";
            inspectText.text = "Premi E per Esaminare";
        }
        if (inspectMode && equiped)
        {
            inspectText.text = "Premi E per Raccogliere il Rubino";

        }
        else
        {
            toggleInspectPanel(false);

        }
        
    }

    protected override void objectControl() {

        
        
        if (m_examining)
        {
            if (!m_picking)
            {
                m_playerHasObject = m_inventory.findItem(m_itemID);

                if (!m_playerHasObject)
                    m_actionText.text = "CONTINUA";
                else
                    m_actionText.text = m_action;
            }
            else
            {
                m_playerHasObject = true;
                m_actionText.text = "PRENDI";
            }

            m_descriptionText.text = m_description;

            m_actionCanvas.alpha = 1;
            m_activated = true;

            m_examining = false;
        }
    }
    protected override void timerEndActions()
    {
        /*
        SwitchControls();
        SwitchCameras();
		if (m_playerHasObject) {
			m_actionDone = true;
			m_transitor.enabled = false;
		}*/
    }

    

    protected override void EndingClickActions() {
        /*if (m_playerHasObject)
        {
            m_item.SetActive(!m_item.activeSelf);
            if (!m_picking)
                m_inventory.deleteItem(m_itemID);
            else
                m_inventory.addItem(m_itemID);
        }
        else if (!m_examining)
            m_examining = true;
        */
    }

    public override bool loadData(int t_key, ulong t_data)
    {
        return base.loadData(t_key, t_data);
    }
}
