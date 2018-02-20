using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableFiaccola : InteractableObject_Abstract
{
    [SerializeField]
    CameraTransitor m_doorTransitor;
    
        GameObject m_fiaccola;
    CameraTransitor m_fiaccolaTransitor;



    // Use this for initialization
    void Start()
    {
        base.Start();

        if (m_doorTransitor == null)
            Debug.LogError(this.name + ": Impossible to find door transitor");

        m_fiaccola = this.transform.Find("Fiaccola").gameObject;
        if (m_fiaccola != null)
        {
            m_fiaccolaTransitor = m_fiaccola.GetComponent<CameraTransitor>();
            if (m_fiaccolaTransitor == null)
                Debug.LogError(this.name + ": Impossible to find fiaccola transitor");
        }
        else
            Debug.LogError(this.name + ": Impossible to find fiaccola");
        m_equiped = false;

        createData(0);  //index 0, 0 significa che l'interruttore non è ancora attivato
    }

    // Update is called once per frame
    void Update()
    {


        base.Update();

        if (m_equiped && m_fiaccolaTransitor.forward != true)
            m_fiaccolaTransitor.forward = true;
            


        if (m_inspectMode && !m_equiped && Input.GetKeyDown(KeyCode.E) && !m_camerasInTransition)
        {
            m_equiped = true;
            UpdateUI();
        }

    }

    protected override void TransitionOutActions()
    {

        base.TransitionOutActions();

        if(m_equiped && m_doorTransitor.forward != true)
            m_doorTransitor.forward = true;

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
                if (!m_equiped)
                    m_uiManager.ToggleActionPanel(true, "Premi E per Tirare");
                else
                    m_uiManager.ToggleActionPanel(false);
            }

        }


    }

    /*IEnumerator ApriChiudiCoperchio(Vector3 byAngles, float inTime)
    {
        var fromAngle = m_coperchio.transform.rotation;
        var toAngle = Quaternion.Euler(m_coperchio.transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            m_coperchio.transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
    }*/


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

