using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

[Serializable]
class AscendingComparer<TKey> : IComparer<int>
{

    public int Compare(int x, int y)
    {
        return x.CompareTo(y);
    }
}

public class SceneControl : MonoBehaviour
{
    public static SceneControl sceneControl;
    SortedList<int, PersistentData> m_persistentObjectList;
    SortedList<int, UInt64> m_persistentDataList;
    int m_currentSceneIndex;

    // Use this for initialization
    void Awake()
    {
        if (sceneControl == null)
        {
            DontDestroyOnLoad(gameObject);
            sceneControl = this;
            m_currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            m_persistentObjectList = new SortedList<int, PersistentData>(new AscendingComparer<int>());
            m_persistentDataList = new SortedList<int, UInt64>(new AscendingComparer<int>());
        }
        else
            Destroy(gameObject);
    }

    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneControl.sceneControl.Save();
            SceneManager.LoadScene(m_currentSceneIndex);
        }
            
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Create);

        m_persistentDataList.Clear();
        foreach (KeyValuePair<int, PersistentData> persistent in m_persistentObjectList) {
            foreach (Data data in persistent.Value.objectData) {
                m_persistentDataList.Add(data.key, data.data);
            }
        }

        bf.Serialize(file, m_persistentDataList);
        file.Close();
    }

	public void ReloadScene(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            m_persistentDataList.Clear();
            m_persistentDataList = (SortedList<int, UInt64>)bf.Deserialize(file);

            int i = 0;
            //modifica i valori corrispondenti dei vari gameobject
            foreach (KeyValuePair<int, PersistentData> persistent in m_persistentObjectList) {

                //1000 index per ogni scena di cui 10 dedicati ad oggetto
                int minimumKey = 1000 * m_currentSceneIndex + 10 * persistent.Key;
                int maximumKey = minimumKey + 10;

                for (; 
                    (i < m_persistentDataList.Count) && 
                    ( 
                        (m_persistentDataList.Keys[i] >= minimumKey) && 
                        (m_persistentDataList.Keys[i] < maximumKey)
                    ); 
                    i++) {


                    if (!persistent.Value.loadData(m_persistentDataList.Keys[i], m_persistentDataList.Values[i]))
                        Debug.LogError("Impossible to load values of key: " + m_persistentDataList.Keys[i]);
                }
            }

            file.Close();
        }
        else
            Debug.Log("Impossible to load save data");
    }

    //scene loading
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        PersistentData.objectCount = 0;

        m_persistentObjectList.Clear();
        PersistentData[] persistentList = FindObjectsOfType<PersistentData>();
        foreach (PersistentData persistent in persistentList)
        {
            m_persistentObjectList.Add(persistent.objectKey, persistent);
        }

        Invoke("Load", 0.1f);
    }
}
