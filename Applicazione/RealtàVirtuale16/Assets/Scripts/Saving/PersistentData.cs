using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

[Serializable]
public class Data {

    public int key;
    public UInt64 data;

    public Data(int t_key, UInt64 t_data) { key = t_key; data = t_data; }
}

/*  Questa classe è la base da cui far ereditare tutti gli script che devono implementare il salvataggio. In particolare 
 *  dovrai fare l'override della funzione loadData, in modo da implementare di volta in volta il modo specifico in cui
 *  l'oggetto usa i dati memorizzati (vedere esempio nella classe BasicInteraction)
 *  
 *  ATTENZIONE per via dell'impostazione che ho dato chiunque utilizzi questa interfaccia può salvare al massimo 
 *  10 valori. Inoltre per evitare overflow possiamo permetterci "solo" 4 milioni di scene con al massimo 100
 *  GameObject che salvano valori in ciascuna. Ricapitolando: 4mln di scene con 100 oggetti da 10 valori ciascuno */
public abstract class PersistentData : MonoBehaviour
{

    [SerializeField]
    int objectID;

    int m_objectNumberKey;

    List<Data> m_dataValues;
    public static int objectCount = 0;
    int m_sceneIndex;
    int m_actualKey;
    int m_indexKey = 0;

    public int objectKey { get { return m_actualKey; } }
    public List<Data> objectData { get { return m_dataValues; }
                                   set { m_dataValues = value; }}
    public PersistentData() {  }

    protected void Awake()
    {
        m_objectNumberKey = objectID;
        m_dataValues = new List<Data>();
        m_sceneIndex = SceneManager.GetActiveScene().buildIndex;
        m_actualKey = 100 * m_sceneIndex + m_objectNumberKey;
    }

    //remember: t_key should be < 0
    public virtual void createData(UInt64 t_data) {

        Data newData = new Data( (10 * m_actualKey + m_indexKey++), t_data);
        if (m_dataValues.Count < 10)
        {
            m_dataValues.Add(newData);
        }
    }

    public virtual void saveData(int t_index, UInt64 t_data) {
        m_dataValues[t_index].data = t_data;
    }
    public virtual bool loadData(int t_key, UInt64 t_data) {
        bool find = false;
        foreach (Data data in m_dataValues) {
            if (data.key == t_key) {
                data.data = t_data;
                find = true;
                break;                    
            }
        }

        return find;
    }

}
