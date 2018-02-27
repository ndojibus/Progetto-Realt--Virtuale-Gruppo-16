using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour {

    [SerializeField]
    private string m_newSceneName;

    public GameObject LoadingPanel;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LoadingPanel.SetActive(true);
            SceneControl.sceneControl.LoadNewScene(m_newSceneName);
        }
    }
}
