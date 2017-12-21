using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePickup : InteractableObject_Abstract
{
    [SerializeField]
    GameObject m_item;

    [SerializeField]
    UInt64 m_itemID = 0;

    [SerializeField]
    bool m_picking = false;

    PlayerInventory m_inventory;
    bool m_playerHasObject = false;
    bool m_examining = false;

    // Use this for initialization
    void Start () {
        base.Start();

        if (m_item == null)
            Debug.LogError("Select an item!");

        GameObject sceneManager = GameObject.Find("SceneManager");
        if (sceneManager != null)
        {
            m_inventory = sceneManager.GetComponent<PlayerInventory>();
            if (m_inventory == null)
                Debug.LogError("Impossible to find a PlayerInventory!");
        }
        else
            Debug.LogError("Impossible to find a scene manager!");
    }

    // Update is called once per frame
    void Update () {
        base.Update();
	}

    protected override void setActionText() {
        m_actionText.text = "ESAMINA";
    }

    protected override void objectControl() {
        if (m_examining)
        {
            if (!m_picking)
            {
                m_playerHasObject = m_inventory.findItem(m_itemID);

                if (!m_playerHasObject)
                    m_actionText.text = "CONTINUA";
                else
                    m_actionText.text = m_action;
            }
            else
                m_actionText.text = "PRENDI";

            m_descriptionText.text = m_description;

            m_actionCanvas.alpha = 1;
            m_activated = true;

            m_examining = false;
        }
    }
}
