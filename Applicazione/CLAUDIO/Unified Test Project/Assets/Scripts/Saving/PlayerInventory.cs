using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerInventory : PersistentData
{
    [SerializeField]
    List<UInt64> m_itemIDList;

    // Use this for initialization
    void Start() {
        m_itemIDList = new List<UInt64>(4);
        createData(0);  //index 0
        createData(0);  //index 1
        createData(0);  //index 2
        createData(0);  //index 3
    }

    // Update is called once per frame
    void Update() {
    }

    public void addItem(UInt64 t_itemID)
    {
        saveData(m_itemIDList.Count, t_itemID);
        m_itemIDList.Add(t_itemID);
        Debug.Log("Added item " + t_itemID);
    }

    public void deleteItem(UInt64 t_itemID)
    {
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
            m_itemIDList[t_key] = t_data;
        }
        return find;
    }
}
