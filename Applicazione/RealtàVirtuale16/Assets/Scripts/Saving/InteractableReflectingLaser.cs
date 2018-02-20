using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableReflectingLaser : InteractableObject_Abstract
{
    [SerializeField]
    private bool m_isActive = false;

    LaserBehaviour m_laser;

    private void Awake()
    {

        base.Awake();

        m_laser = GetComponentInChildren<LaserBehaviour>();
        if (m_laser == null)
            Debug.LogError(this.name + ": " + "Impossible to find LaserBehaviour!");

    }

    // Use this for initialization
    protected void Start()
    {
        base.Start();

        //m_item.SetActive(true);
        m_equiped = m_isActive;
        m_inspectMode = false;
    }

    // Update is called once per frame
    protected void Update()
    {

        if (m_inspectMode == false && m_equiped == true && m_equiped != m_laser.laserActive && !m_camerasInTransition)
        {

            EnableLaser();
            ChangeTransitor(1);

        }
        else if (m_inspectMode == false && m_equiped == false && m_equiped != m_laser.laserActive && !m_camerasInTransition)
        {

            DisableLaser();
            ChangeTransitor(0);

        }

        base.Update();

    }

    private void EnableLaser()
    {
        m_laser.laserActive = m_equiped;
        BoxCollider currentTrigger = this.GetComponent<BoxCollider>();
        if (currentTrigger != null && currentTrigger.isTrigger)
        {
            currentTrigger.size = new Vector3(5f, 0.5f, 5f);
            currentTrigger.center = new Vector3(0f, 0.5f, -1f);
        }
        else
        {
            Debug.LogError(this.name + ": couldn't find any trigger");
        }
    }

    private void DisableLaser()
    {
        m_laser.laserActive = m_equiped;
        BoxCollider currentTrigger = this.GetComponent<BoxCollider>();
        if (currentTrigger != null && currentTrigger.isTrigger)
        {
            currentTrigger.size = new Vector3(1f, 0.5f, 4f);
            currentTrigger.center = new Vector3(0f, 0.5f, 2f);
        }
        else
        {
            Debug.LogError(this.name + ": couldn't find any trigger");
        }
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

        if (!m_camerasInTransition)
        {
            if (m_inspectMode)
            {

                if (m_equiped && !m_laser.laserActive)
                {
                    m_uiManager.ToggleActionPanel(false);
                    m_uiManager.ToggleDescriptionPanel(false);
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
    }



    protected override void SwitchControls()
    {
        base.SwitchControls();
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
