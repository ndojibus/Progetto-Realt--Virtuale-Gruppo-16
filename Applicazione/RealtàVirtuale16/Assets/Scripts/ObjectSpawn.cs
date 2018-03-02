using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour {

    [SerializeField]
    private List<GameObject> m_objectList;

    [SerializeField]
    private string m_triggerTag = "Player";

    public AudioClip actionSound;
    public string m_tutorial;
    private AudioSource source;

    public UIManager m_uiManagaer;


    private void Awake()
    {
        source = this.GetComponent<AudioSource>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == m_triggerTag)
        {

            //actionsound

            source.PlayOneShot(actionSound, 1f);

            //tutorial

            m_uiManagaer.SetDescriptionText(m_tutorial);
            m_uiManagaer.ActiveDescriptionPanel();

            //apparelamummia
            foreach (GameObject obj in m_objectList)
                obj.SetActive(!obj.activeSelf);
            this.gameObject.SetActive(false);
        }

        
    }


    private void OnTriggerExit(Collider other)
    {

        
        m_uiManagaer.DisableDescriptionPanel();
    }
}
