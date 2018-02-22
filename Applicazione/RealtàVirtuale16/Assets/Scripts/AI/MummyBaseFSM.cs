using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MummyBaseFSM : StateMachineBehaviour
{
    [SerializeField]
    float m_visibilityRange = 10f;

    [Range(0, 45)]
    [SerializeField]
    float m_variableAngleFoward = 45;

    protected GameObject m_player;
    protected GameObject m_mummy;
    protected PatrolList m_mummyPatrolList;
    protected List<Waypoint> m_patrolPoints;
    protected NavMeshAgent m_navMeshAgent;
    

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
                m_mummyPatrolList.fowardRayDistance = m_visibilityRange;
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

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!m_mummyPatrolList.canChase) {
            animator.SetBool("canChase", false);
        }
    }

    protected bool checkPlayerVisibility()
    {
        bool isHit = false;
        RaycastHit playerHit;
        Vector3 startRaycastPosition = new Vector3(m_mummy.transform.position.x, m_mummy.transform.position.y + 1f, m_mummy.transform.position.z);
        
        float horizontalAngle = Random.Range(-m_variableAngleFoward, m_variableAngleFoward);
        float verticalAngle = Random.Range(-m_variableAngleFoward/2, m_variableAngleFoward/2);
        Vector3 rotatedVector = Quaternion.AngleAxis(horizontalAngle, m_mummy.transform.up) * m_mummy.transform.forward;
        rotatedVector = Quaternion.AngleAxis(verticalAngle, m_mummy.transform.right) * rotatedVector;
        m_mummyPatrolList.gizmoFowardDirection = rotatedVector;
        if (Physics.Raycast(startRaycastPosition, rotatedVector, out playerHit, m_visibilityRange))
        {
            if (playerHit.collider.tag == "Player")
                isHit = true;
        }

        return isHit;
    }
}
