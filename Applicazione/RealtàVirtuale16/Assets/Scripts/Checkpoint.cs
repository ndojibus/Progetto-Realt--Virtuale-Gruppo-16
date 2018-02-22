using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    [SerializeField]
    bool m_vanish = true;

    bool m_activated = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && m_activated) {
            SavePlayerPosition save = other.GetComponent<SavePlayerPosition>();
            if (save != null)
                save.savePlayerPosition();
            else
                Debug.LogError(this.name + ": Impossible to find player save position!");

            SceneControl.sceneControl.Save();

            if (m_vanish)
                m_activated = false;
        }
    }
}
