using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableOpen : InteractableObject_Abstract
{


    GameObject m_item;
    GameObject m_coperchio;

    private bool is_opened=false;

    public AudioClip openSound;
    private AudioSource source;

    private float volLowRange = .5f;
    private float volHighRange = 1.0f;


    private void Awake()
    {
        base.Awake();
        source = this.GetComponent<AudioSource>();

    }

    // Use this for initialization
    void Start()
    {
        base.Start();



        m_item = this.transform.Find("Rubino").gameObject;
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

            TakeRuby();
            UpdateUI();


        }

   
        if (m_inspectMode && !is_opened)
        {
            source.PlayOneShot(openSound, 1f);
            //m_coperchio.transform.Rotate(-90, 0, 0);
            //m_coperchio.transform.RotateAround(m_rotateAround.transform.position, new Vector3(0, 0, 1), -2);
            StartCoroutine(ApriChiudiCoperchio(new Vector3(1,0,0) * -90, m_cameraSwitchTime));

            is_opened = true;
            
        }
        if (!m_inspectMode && is_opened)
        {
            StartCoroutine(ApriChiudiCoperchio(new Vector3(1, 0, 0) * 90, m_cameraSwitchTime));
            //m_coperchio.transform.RotateAround(m_rotateAround.transform.position, new Vector3(0, 0, 1), 2);
            source.PlayOneShot(openSound, 1f);
            is_opened = false;

        }

        








    }

    IEnumerator ApriChiudiCoperchio(Vector3 byAngles, float inTime)
    {
        var fromAngle = m_coperchio.transform.rotation;
        var toAngle = Quaternion.Euler(m_coperchio.transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            m_coperchio.transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
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
