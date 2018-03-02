using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneControl.sceneControl.DeleteSave();
            SceneControl.sceneControl.LoadNewScene("MainMenuScene");

        }
    }
}
