using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameOnClick : MonoBehaviour {

    public void NewGame(string scene)
    {
        SceneControl.sceneControl.DeleteSave();
        SceneControl.sceneControl.LoadNewScene(scene);
    }
}
