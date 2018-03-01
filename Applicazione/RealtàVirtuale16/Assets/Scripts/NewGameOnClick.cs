using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameOnClick : MonoBehaviour {

    public void NewGame(int scene)
    {
        SceneControl.sceneControl.DeleteSave();
        SceneManager.LoadScene(scene);


    }
}
