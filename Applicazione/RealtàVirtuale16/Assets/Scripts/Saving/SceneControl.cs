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

    UIManager m_uiManager;

    string m_newSceneName;
    bool m_newSceneLoading = false;

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

        if (Input.GetKeyDown(KeyCode.T))
        {

            DeleteSave();
            SceneManager.LoadScene(m_currentSceneIndex);
        }

    }


    
    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Create);

        m_persistentDataList.Clear();
        m_persistentDataList.Add(-1, (ulong) SceneManager.GetActiveScene().buildIndex);
        foreach (KeyValuePair<int, PersistentData> persistent in m_persistentObjectList) {
            foreach (Data data in persistent.Value.objectData) {
                //Debug.Log("Saved values of key " + data.key + " and value " + data.data + " for object " + persistent.Value.name);
                m_persistentDataList.Add(data.key, data.data);
            }
        }

        bf.Serialize(file, m_persistentDataList);
        file.Close();
    }

	public void ReloadScene(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void DeleteSave() {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
            File.Delete(Application.persistentDataPath + "/playerInfo.dat");
        }
    }

    public void LoadNewScene(string name)
    {
        m_uiManager.ActiveLoadingPanel(0.3f);
        m_newSceneName = name;
        Invoke("LoadSceneByName", 0.3f);
        m_newSceneLoading = true;
    }

    private void LoadSceneByName() {
        SceneManager.LoadScene(m_newSceneName);
    }

    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            m_persistentDataList.Clear();
            m_persistentDataList = (SortedList<int, UInt64>)bf.Deserialize(file);

            if (m_currentSceneIndex != 0)
            {
                if (!m_newSceneLoading)
                {
                    if (m_persistentDataList.Values[0] != (ulong)SceneManager.GetActiveScene().buildIndex)
                        SceneManager.LoadScene((int)m_persistentDataList.Values[0]);

                    int i = 1;
                    //modifica i valori corrispondenti dei vari gameobject
                    foreach (KeyValuePair<int, PersistentData> persistent in m_persistentObjectList)
                    {

                        //1000 index per ogni scena di cui 10 dedicati ad oggetto
                        int minimumKey = 1000 * m_currentSceneIndex + 10 * (persistent.Key - m_currentSceneIndex * 100);
                        int maximumKey = minimumKey + 10;

                        for (;
                            (i < m_persistentDataList.Count) &&
                            (
                                (m_persistentDataList.Keys[i] >= minimumKey) &&
                                (m_persistentDataList.Keys[i] < maximumKey)
                            );
                            i++)
                        {


                            if (!persistent.Value.loadData(m_persistentDataList.Keys[i], m_persistentDataList.Values[i]))
                                Debug.LogError("Impossible to load values of key " + m_persistentDataList.Keys[i] + " and value " + m_persistentDataList.Values[i] + " from " + persistent.Value.name);
                            /*else
                                Debug.Log("Loaded values of key " + m_persistentDataList.Keys[i] + " and value " + m_persistentDataList.Values[i] + " from " + persistent.Value.name);*/
                        }
                    }
                    file.Close();

                }
                else
                {
                    m_newSceneLoading = false;
                    file.Close();
                    Save();
                }

            }
            else
                Debug.Log("Impossible to load save data");
        }

        m_uiManager.DeactiveLoadingPanel(0.5f);
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
        m_uiManager = FindObjectOfType<UIManager>();
        if (m_uiManager == null)
            Debug.LogError("Impossible to find UImanager");

        m_currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        PersistentData.objectCount = 0;

        m_persistentObjectList.Clear();
        PersistentData[] persistentList = FindObjectsOfType<PersistentData>();
        foreach (PersistentData persistent in persistentList)
        {
            Debug.Log(persistent.objectKey + " " + persistent.name);
            m_persistentObjectList.Add(persistent.objectKey, persistent);        
        }

        Invoke("Load", 0.1f);
        if (Time.timeScale != 1f)
            Time.timeScale = 1f;
    }
}
