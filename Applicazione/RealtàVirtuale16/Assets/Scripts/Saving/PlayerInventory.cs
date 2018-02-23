using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class PlayerInventory : PersistentData
{
    [SerializeField]
    List<UInt64> m_itemIDList;


    GameObject m_inventoryPanel;
    Image m_RubyIcon;
    Image m_KeyIcon;
    Image m_MoneyBagIcon;

    void Awake()
    {
        base.Awake();

        GameObject canvas = GameObject.Find("HUDCanvas");
        if (canvas != null)
        {

            m_inventoryPanel = canvas.transform.Find("InventoryPanel").gameObject;
            m_RubyIcon = m_inventoryPanel.GetComponentsInChildren<Image>()[1];
            m_KeyIcon = m_inventoryPanel.GetComponentsInChildren<Image>()[2];
            m_MoneyBagIcon = m_inventoryPanel.GetComponentsInChildren<Image>()[3];
            
            if (m_inventoryPanel == null || m_RubyIcon == null || m_KeyIcon == null || m_MoneyBagIcon)
                Debug.LogError(this.name + ": " + "Impossible to find inventory!");

            
        }

        
    }
    // Use this for initialization
    void Start() {

        m_RubyIcon.enabled = false;
        m_KeyIcon.enabled = false;
        m_MoneyBagIcon.enabled = false;
        m_itemIDList = new List<UInt64>(4);

        //il primo item è il rubino
        m_itemIDList.Add(2); 
        createData(0);  //index 0
        
        createData(0);  //index 1
        //createData(0);  //index 2
        //createData(0);  //index 3
    }

    public bool HasRuby()
    {
        return m_RubyIcon.enabled;
    }

    public bool HasKey()
    {

        return m_KeyIcon.enabled;
    }

    public bool HasMoneyBag()
    {
        return m_MoneyBagIcon.enabled;
    }

    public void PickRuby()
    {
        m_RubyIcon.enabled = true;
        saveData(0, 1);
    }

    public void UseRuby()
    {
        m_RubyIcon.enabled = false;
        saveData(0, 0);
    }

    public void PickKey()
    {
        m_KeyIcon.enabled = true;
        saveData(1, 1);
    }

    public void UseKey()
    {
        m_KeyIcon.enabled = false;
        saveData(1, 0);
    }

    public void PickMoneyBag()
    {
        m_MoneyBagIcon.enabled= true;
        saveData(2, 1);
    }

    public void UseMoneyBag()
    {
        m_MoneyBagIcon.enabled = false;
        saveData(2, 0);
    }


    public void addItem(UInt64 t_itemID)
    {

        saveData(m_itemIDList.Count, t_itemID);
        m_itemIDList.Add(t_itemID);
        Debug.Log("Added item " + t_itemID);
    }

    public void deleteItem(UInt64 t_itemID)
    {

        m_RubyIcon.enabled = false;
        m_itemIDList.Remove(t_itemID);

        for (int i = 0; i < m_itemIDList.Count; i++) {
            saveData(i, t_itemID);
        }
        Debug.Log("Deleted item " + t_itemID);
    }

    public bool findItem(UInt64 t_itemID){
        bool find = false;

        foreach (UInt64 id in m_itemIDList) {
            if (id == t_itemID)
            {
                find = true;
                break;
            }
        }

        return find;
    }

    public override bool loadData(int t_key, UInt64 t_data)
    {
        bool find = base.loadData(t_key, t_data);
        if (((int)t_data >= 0) && find)
        {
            int inventoryNumber = t_key - (objectKey * 10);
            //se t_key è POI VEDIAMO allora sta caricando il rubino, se t_data è 1 significa che il rubino è presente nell'inventario
            if (inventoryNumber == 0 && t_data == 1)
                m_RubyIcon.enabled = true;
            else if (inventoryNumber == 1 && t_data == 1)
                m_KeyIcon.enabled = true;
        }
        return find;
    }
}
