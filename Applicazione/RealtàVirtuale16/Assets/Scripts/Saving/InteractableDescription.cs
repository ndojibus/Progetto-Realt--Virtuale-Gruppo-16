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

    protected override void SetAction()
    {
        if (!m_inspectMode)
        {
            m_uiManager.SetActionText("Premi E per Esaminare");
            m_uiManager.ToggleActionPanel(true);
            
            
        }
        else
        {
            m_uiManager.ToggleActionPanel(false);

        }
    }


    protected override void objectControl()
    {
    }
    protected override void timerEndActions()
    {
    }

    protected override void transitionInActions()
    {
        base.transitionInActions();
        m_uiManager.ToggleActionPanel(false);



    }

    protected override void EndingClickActions()
    {
    }

    public override bool loadData(int t_key, ulong t_data)
    {
        return base.loadData(t_key, t_data);
    }
}
