using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDescription : InteractableObject_Abstract
{

    

    // Use this for initialization
    void Start()
    {
        base.Start();

        m_equiped = true;
    }

    // Update is called once per frame
    void Update()
    {


        base.Update();

    }

    

    protected override void UpdateUI()
    {

        

        base.UpdateUI();

        if (!m_inspectMode)
            m_actionText = "Premi E per esaminare";

        if (m_inspectMode)
            m_uiManager.ToggleActionPanel(false);

    }


    public override bool loadData(int t_key, ulong t_data)
    {
        return base.loadData(t_key, t_data);
    }
}
