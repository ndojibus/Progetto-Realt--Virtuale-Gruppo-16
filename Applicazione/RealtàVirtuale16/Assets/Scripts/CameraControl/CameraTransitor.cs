using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraTransition {
    public GameObject transitionPoint;
    public float transitionSpeed;
}

public class CameraTransitor : MonoBehaviour {

    [SerializeField]
    private int m_transitorID;

    [SerializeField]
    private List<CameraTransition> m_transitionList;

    [SerializeField]
    private bool m_forward = true;

    [SerializeField]
    private bool m_moving = true;

    private float m_currentTransitionPoint = 0;
    private int m_currentTransitionIndex = 0;

    private bool m_initialized = false;
    
    public bool moving
    {
        get { return m_moving; }
        set { m_moving = value; }
    }

    public bool forward
    {
        get { return m_forward; }
        set { m_forward = value; }
    }

    public int transitorID {
        get { return m_transitorID;  }
        set { m_transitorID = value; }
    }

    // Use this for initialization
    void Start () {
        if (m_transitionList != null && m_transitionList.Count >= 2)
            m_initialized = true;
        else
            Debug.LogError(this.name + ": " + "transitionList should have at least 2 elements!");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_initialized && m_moving)
        {
            if ((m_currentTransitionPoint >= 1f) && m_forward)
            {
                m_currentTransitionIndex++;
                m_currentTransitionPoint = 0f;

            } else if ((m_currentTransitionPoint <= 0f) && !m_forward)
            {
                m_currentTransitionIndex--;
                m_currentTransitionPoint = 1f;
            }



            if ((m_currentTransitionIndex + 1 < m_transitionList.Count) && (m_currentTransitionIndex >= 0))
            {

                var currentTransition = m_transitionList[m_currentTransitionIndex];
                var nextTransition = m_transitionList[m_currentTransitionIndex + 1];

                if (m_forward)
                    m_currentTransitionPoint += Time.deltaTime * currentTransition.transitionSpeed;
                else
                    m_currentTransitionPoint -= Time.deltaTime * nextTransition.transitionSpeed;

                m_currentTransitionPoint = Mathf.Clamp(m_currentTransitionPoint, 0f, 1f);

                transform.position = Vector3.Lerp(currentTransition.transitionPoint.transform.position,
                                        nextTransition.transitionPoint.transform.position,
                                        m_currentTransitionPoint);

                transform.localRotation = Quaternion.Lerp(currentTransition.transitionPoint.transform.rotation,
                                                    nextTransition.transitionPoint.transform.rotation,
                                                    m_currentTransitionPoint);
            }
            else if (m_currentTransitionIndex + 1 >= m_transitionList.Count)
            {
                var currentTransition = m_transitionList[m_transitionList.Count - 2];
                var nextTransition = m_transitionList[m_transitionList.Count - 1];

                transform.position = Vector3.Lerp(currentTransition.transitionPoint.transform.position,
                                        nextTransition.transitionPoint.transform.position,
                                        1f);

                transform.localRotation = Quaternion.Lerp(currentTransition.transitionPoint.transform.rotation,
                                                    nextTransition.transitionPoint.transform.rotation,
                                                    1f);
            }
            else if(m_currentTransitionIndex < 0)
            {
                    var currentTransition = m_transitionList[0];
                    var nextTransition = m_transitionList[1];

                    transform.position = Vector3.Lerp(currentTransition.transitionPoint.transform.position,
                                            nextTransition.transitionPoint.transform.position,
                                            0f);

                    transform.localRotation = Quaternion.Lerp(currentTransition.transitionPoint.transform.rotation,
                                                        nextTransition.transitionPoint.transform.rotation,
                                                        0f);
            }


        }
    }
}
