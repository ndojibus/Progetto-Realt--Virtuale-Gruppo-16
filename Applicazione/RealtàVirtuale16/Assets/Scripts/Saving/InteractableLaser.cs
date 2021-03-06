﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLaser : InteractableObject_Abstract {

    [SerializeField]
    bool m_active = false;

    [SerializeField]
    string m_description2 = "";

    LaserBehaviour m_laser;
    GameObject m_item;

    public AudioClip putInSound;
    private AudioSource source;

    private void Awake()
    {

        base.Awake();

        m_laser = GetComponentInChildren<LaserBehaviour>();
        if (m_laser == null)
            Debug.LogError(this.name + ": " + "Impossible to find LaserBehaviour!");

        m_item = this.transform.Find("LaserStart").Find("Rubino").gameObject;
        if (m_item == null)
            Debug.LogError(this.name + ": " + "Select an item!");

        source = this.GetComponent<AudioSource>();
    }

    // Use this for initialization
    protected void Start () {
		base.Start ();
      
        //m_item.SetActive(true);
        m_equiped = m_active;
        m_inspectMode = false;

        createData((ulong)(m_equiped ? 1 : 0));      //index 0, inizializzato a 0 perché non c'è il rubino

        float angleToLong = (m_laser.transform.rotation.eulerAngles.y + 3600f) * 100f;
        createData((ulong)angleToLong);      //index 1, rotazione del laser
	}
	
	// Update is called once per frame
	protected void Update () {

        if (m_inspectMode == false && m_equiped == true && m_equiped != m_laser.laserActive && !m_camerasInTransition)
        {
            EnableLaser();

            m_description = m_description2;
            
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
        m_laser.laserActive = m_equiped;
        BoxCollider currentTrigger = this.GetComponent<BoxCollider>();
        if (currentTrigger != null && currentTrigger.isTrigger)
        {
            currentTrigger.size = new Vector3(5f, 3f, 5f);
            currentTrigger.center = new Vector3(0f, 1.5f, -1f);
        }
        else {
            Debug.LogError(this.name + ": couldn't find any trigger");
        }
    }

    private void InsertRuby()
    {

        source.PlayOneShot(putInSound, 1f);
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

        if (!m_camerasInTransition) { 
            if (m_inspectMode)
            {
            
                if (m_equiped && !m_laser.laserActive)
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








    }


    

    

    protected override void TransitionOutActions()
    {
        base.TransitionOutActions();

        float angleToLong = (m_laser.transform.rotation.eulerAngles.y + 3600f) * 100f;
        saveData(1, (ulong)angleToLong);
    }



    protected override void SwitchControls()
	{
		base.SwitchControls ();
		m_laser.controlled = !m_laser.controlled;
	}

    public void saveRotation() {
        float angleToLong = (m_laser.transform.rotation.eulerAngles.y + 3600f) * 100f;
        saveData(1, (ulong)angleToLong);
    }

    public override bool loadData(int t_key, ulong t_data)
    {
        bool find = base.loadData(t_key, t_data);
        if ((int)t_data >= 0 && find)
        {
            int inventoryNumber = t_key - (objectKey * 10);
            if (t_data == 1 && inventoryNumber == 0)
                m_equiped = true;
            else if (inventoryNumber == 1)
            {
                float newRotation = (float)(t_data * 0.01f - 3600f);
                m_laser.transform.RotateAround(m_laser.transform.position, m_laser.transform.up, newRotation);
            }
        }
        return find;
    }
}
