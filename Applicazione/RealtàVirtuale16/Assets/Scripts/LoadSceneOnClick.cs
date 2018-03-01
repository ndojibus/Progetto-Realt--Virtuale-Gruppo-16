
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {


    public void NewGame()
    {
        SceneControl.sceneControl.DeleteSave();
        SceneManager.LoadScene(1);
        

    }


    
}
