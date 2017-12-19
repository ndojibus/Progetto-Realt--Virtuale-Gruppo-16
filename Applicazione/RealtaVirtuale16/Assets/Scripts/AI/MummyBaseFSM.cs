using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MummyBaseFSM : StateMachineBehaviour
{
    [SerializeField]
    float m_visibilityRange = 10f;

    GameObject m_player;
    GameObject m_mummy;
    PatrolList m_mummyPatrolList;
    List<Waypoint> m_patrolPoints;
    NavMeshAgent m_navMeshAgent;
    

    bool m_initialized;

    protected GameObject mummy { get { return m_mummy; } }
    protected GameObject player { get { return m_player; } }
    protected PatrolList mummyPatrolList { get { return m_mummyPatrolList; } }
    protected List<Waypoint> patrolPoints { get { return m_patrolPoints; } }
    protected NavMeshAgent navMeshAgent { get { return m_navMeshAgent; } }
    protected bool initialized { get { return m_initialized; } }


    // Use this for initialization
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_initialized = false;
        m_mummy = animator.gameObject;
        if (m_mummy == null)
            Debug.LogError("No gameobject is attached to " + animator.name);
        else
        {
            m_navMeshAgent = m_mummy.GetComponent<NavMeshAgent>();
            m_mummyPatrolList = m_mummy.GetComponent<PatrolList>();
            if (m_navMeshAgent == null || m_mummyPatrolList == null)
            {
                Debug.LogError("The nav mesh agent component or patrol list agent is not attached to " + m_mummy.name);
            }
            else
            {
                m_patrolPoints = m_mummyPatrolList.patrolPoints;
                m_mummyPatrolList.rayDistance = m_visibilityRange;
                if (m_patrolPoints != null && m_patrolPoints.Count >= 2)
                {
                    m_player = GameObject.FindGameObjectWithTag("Player");
                    if (m_player != null)
                        m_initialized = true;
                    else
                        Debug.LogError("No player find!");
                }
                else
                    Debug.LogError("Not enough patrol points!");
            }
        }
    }

    protected bool checkPlayerVisibility()
    {
        bool isHit = false;
        RaycastHit playerHit;
        if (Physics.Raycast(m_mummy.transform.position, m_mummy.transform.forward, out playerHit, m_visibilityRange))
        {
            if (playerHit.collider.tag == "Player")
                isHit = true;
        }

        return isHit;
    }
}
