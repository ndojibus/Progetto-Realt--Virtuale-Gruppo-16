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


    GameObject inventoryPanel;
    Image RubyIco;

    void Awake()
    {
        base.Awake();

        GameObject canvas = GameObject.Find("HUDCanvas");
        if (canvas != null)
        {

            inventoryPanel = canvas.transform.Find("InventoryPanel").gameObject;
            RubyIco = inventoryPanel.GetComponentsInChildren<Image>()[1];
            if (inventoryPanel == null || RubyIco == null)
                Debug.LogError("Impossible to find inventory!");

            RubyIco.enabled = false;
        }
    }
    // Use this for initialization
    void Start() {
        m_itemIDList = new List<UInt64>(4);

        //il primo item è il rubino
        m_itemIDList.Add(2); 
        createData(0);  //index 0
        
        //createData(0);  //index 1
        //createData(0);  //index 2
        //createData(0);  //index 3
    }

    public bool HasRuby()
    {
        return RubyIco.enabled;
    }

    public void PickRuby()
    {
        RubyIco.enabled = true;
        saveData(0, 1);
    }

    public void UseRuby()
    {
        RubyIco.enabled = false;
        saveData(0, 0);
    }
    

    public void addItem(UInt64 t_itemID)
    {

        saveData(m_itemIDList.Count, t_itemID);
        m_itemIDList.Add(t_itemID);
        Debug.Log("Added item " + t_itemID);
    }

    public void deleteItem(UInt64 t_itemID)
    {

        RubyIco.enabled = false;
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
            //se t_key è POI VEDIAMO allora sta caricando il rubino, se t_data è 1 significa che il rubino è presente nell'inventario
            if (t_data == 1)
                RubyIco.enabled = true;
        }
        return find;
    }
}
