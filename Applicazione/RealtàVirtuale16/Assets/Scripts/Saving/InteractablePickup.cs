﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePickup : InteractableObject_Abstract
{
    
    GameObject m_item;

    // Use this for initialization
    void Start () {
        base.Start();



        m_item = this.transform.Find("Rubino").gameObject;
        if (m_item == null)
            Debug.LogError("Select an item!");

        m_equiped = true;

        createData(1);  //index 0, 1 significa che il rubino è ancora incastonato
    }

    // Update is called once per frame
    void Update () {


        base.Update();

        if (m_equiped != m_item.activeSelf)
            m_item.SetActive(m_equiped);


        if (m_inspectMode && m_equiped && Input.GetKeyDown(KeyCode.E) && !m_camerasInTransition)
        {
            
            TakeRuby();
            UpdateUI();
            

        }

	}

    private void TakeRuby()
    {

        //inserito nell'inventario
        m_inventory.PickRuby();

        //cancellato dalla scena

        m_item.SetActive(false);
        m_equiped = false;

        saveData(0, 0);   // salva che il rubino è stato preso
    }

    

    protected override void UpdateUI()
    {

        // ***NON INSPECT MODE***
         if (!m_inspectMode)
        {
           
            m_actionText = "Premi E per Esaminare";
            
        }

        base.UpdateUI();

        // *** INSPECT MODE ***
        if (!m_camerasInTransition)
        {
            if (m_inspectMode)
            {
                if (m_equiped)
                    m_uiManager.ToggleActionPanel(true, "Premi E per Raccogliere il Rubino");
                else
                    m_uiManager.ToggleActionPanel(false);
            }

        }
        
      

    }
    

    public override bool loadData(int t_key, ulong t_data)
    {
        bool find = base.loadData(t_key, t_data);
        if (t_data == 0 && find)
        {
            m_equiped = false;
            
        }
        return find;
    }
}
