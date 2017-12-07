using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyHitPlayer : MonoBehaviour {
    [SerializeField]
    float m_deathScreenSpeed = 1f;

    CanvasGroup m_UI;
    bool m_dead = false;

    void Awake() {
        GameObject canvas = GameObject.FindGameObjectWithTag("UI");
        if (canvas != null)
        {
            m_UI = canvas.GetComponent<CanvasGroup>();
            if(m_UI == null)
                Debug.LogError("No canvas group find!");
        }
        else
            Debug.LogError("No UI find!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            m_dead = true;
        }
    }

    void Update() {
        if (m_dead && (m_UI.alpha < 1))
        {
            m_UI.alpha += Time.deltaTime * m_deathScreenSpeed;
        }
    }

}
