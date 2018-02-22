using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePickToggle : InteractableObject_Abstract
{

    [SerializeField]
    CameraTransitor m_doorTransitor;

    GameObject m_pickup1;
    GameObject m_pickup2;

    bool m_noMorePick = false;
    // Use this for initialization
    void Start () {
        base.Start();

        if (m_doorTransitor == null)
            Debug.LogError(this.name + ": No door transitor selected");

        m_pickup1 = this.transform.Find("Pickup1").gameObject;
        if (m_pickup1 == null)
            Debug.LogError(this.name + ": There is no pickup1!");

        m_pickup2 = this.transform.Find("Pickup2").gameObject;
        if (m_pickup2 == null)
            Debug.LogError(this.name + ": There is no pickup2!");

        m_equiped = true;

        createData(1);  //index 0, 1 significa che l'oggetto da prendere è ancora inserito
    }

    // Update is called once per frame
    void Update () {
        if (m_doorTransitor.forward != m_equiped)
            m_doorTransitor.forward = m_equiped;

        base.Update();

        if (!m_noMorePick)
        {
            if (m_inspectMode && m_equiped && !m_inventory.HasKey() && Input.GetKeyDown(KeyCode.E) && !m_camerasInTransition)
            {

                TakeKey();
                UpdateUI();
            }
            else if (m_inspectMode && !m_equiped && m_inventory.HasRuby() && Input.GetKeyDown(KeyCode.E) && !m_camerasInTransition)
            {

                PlaceRuby();
                UpdateUI();


            }
            else if (m_inspectMode && !m_equiped && m_inventory.HasKey() && Input.GetKeyDown(KeyCode.E) && !m_camerasInTransition)
            {

                PlaceKey();
                UpdateUI();


            }
        }

    }

    private void TakeKey()
    {

        //inserito nell'inventario
        m_inventory.PickKey();

        //cancellato dalla scena

        m_pickup1.SetActive(false);
        m_equiped = false;

        saveData(0, 0);   // salva che il primo oggetto è stato preso
    }

    private void PlaceKey()
    {

        //inserito nell'inventario
        m_inventory.UseKey();

        //cancellato dalla scena

        m_pickup1.SetActive(true);
        m_equiped = true;

        saveData(0, 1);   // salva che il primo oggetto è stato tolto
    }

    private void PlaceRuby()
    {

        //inserito nell'inventario
        m_inventory.UseRuby() ;

        //cancellato dalla scena

        m_pickup2.SetActive(true);
        m_equiped = true;
        m_noMorePick = true;

        saveData(0, 2);   // salva che il primo oggetto è stato tolto
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
                if (m_equiped && !m_inventory.HasKey())
                    m_uiManager.ToggleActionPanel(true, "Premi E per Raccogliere la chiave");
                else if(!m_equiped && m_inventory.HasKey() && !m_inventory.HasRuby())
                    m_uiManager.ToggleActionPanel(true, "Premi E per mettere la chiave");
                else if(!m_equiped && m_inventory.HasRuby())
                    m_uiManager.ToggleActionPanel(true, "Premi E per mettere il rubino");
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
        else if (t_data == 2 && find) {
            m_pickup1.SetActive(false);
            m_pickup2.SetActive(true);
            m_equiped = true;
            m_noMorePick = true;
        }
        return find;
    }
}
