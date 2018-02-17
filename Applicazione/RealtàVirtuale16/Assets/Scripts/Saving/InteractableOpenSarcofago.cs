using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableOpenSarcofago : InteractableObject_Abstract
{

    GameObject m_item;
    
    GameObject m_coperchio;

    
    private bool is_opened = false;



    // Use this for initialization
    void Start()
    {
        base.Start();



        m_item = this.transform.Find("Hank").gameObject;
        if (m_item == null)
            Debug.LogError("Select an item!");

        //m_rotateAround = this.transform.Find("RotateAround").gameObject;
        m_coperchio = this.transform.Find("Coperchio").gameObject;

        m_equiped = true;

        createData(1);  //index 0, 1 significa che il rubino è ancora incastonato
    }

    // Update is called once per frame
    void Update()
    {


        base.Update();

        if (m_equiped != m_item.activeSelf)
            m_item.SetActive(m_equiped);


        if (m_inspectMode && m_equiped && Input.GetKeyDown(KeyCode.E) && !m_camerasInTransition)
        {

            TakeKey();
            UpdateUI();


        }


        if (m_inspectMode && !is_opened)
        {
            //m_coperchio.transform.Rotate(-90, 0, 0);
            //m_coperchio.transform.RotateAround(m_rotateAround.transform.position, new Vector3(0, 0, 1), -2);
            StartCoroutine(ApriChiudiCoperchio( m_cameraSwitchTime));
           
            is_opened = true;

        }
        

    }

    IEnumerator ApriChiudiCoperchio( float inTime)
    {

        
        var toPosition = new Vector3(m_coperchio.transform.localPosition.x + 0.55f, m_coperchio.transform.localPosition.y , m_coperchio.transform.localPosition.z );
        //var toPosition = Vector3(m_coperchio.transform.position - new Vector3(0f,0f,-0.7f) );
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            m_coperchio.transform.localPosition = Vector3.Lerp(m_coperchio.transform.localPosition, toPosition, t);
            //m_coperchio.transform.Translate(new Vector3(0,0,-1) * 50f*Time.deltaTime);
            yield return null;
        }
    }

    private void TakeKey()
    {

        //inserito nell'inventario
        m_inventory.PickKey();

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

            m_actionText = "Premi E per Profanare";

        }

        base.UpdateUI();

        // *** INSPECT MODE ***

        if (!m_camerasInTransition)
        {
            if (m_inspectMode)
            {
                if (m_equiped)
                    m_uiManager.ToggleActionPanel(true, "Premi E per Raccogliere la chiave");
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
