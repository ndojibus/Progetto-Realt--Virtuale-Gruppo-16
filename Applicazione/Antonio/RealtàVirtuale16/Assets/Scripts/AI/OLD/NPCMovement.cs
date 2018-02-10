using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour {

    [SerializeField]
    Transform m_destination;

    [SerializeField]
    bool m_patrolWaiting;

    [SerializeField]
    float m_totalWaitTime = 3f;

    [SerializeField]
    float m_switchProbability = 0.2f;

    [SerializeField]
    List<Waypoint> m_patrolPoints;


    NavMeshAgent m_navMeshAgent;
    int m_currentPatrolIndex;
    bool m_travelling;
    bool m_waiting;
    bool m_patrolFoward;
    float m_waitTimer;

	// Use this for initialization
	void Start () {
        m_navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (m_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
        else {
            if (m_patrolPoints != null && m_patrolPoints.Count >= 2)
                m_currentPatrolIndex = 0;
            else 
                m_currentPatrolIndex = -1;

            SetDestination();
        }

    }
	
	// Update is called once per frame
	public void Update () {
		if (m_travelling && m_navMeshAgent.remainingDistance <= 1.0f) {
            m_travelling = false;

            if (m_patrolWaiting)
            {
                m_waiting = true;
                m_waitTimer = 0f;
            }
            else {
                ChangePatrolPoint();
                SetDestination();
            }
        }

        if (m_waiting) {
            m_waitTimer += Time.deltaTime;
            if (m_waitTimer >= m_totalWaitTime) {
                m_waiting = false;

                ChangePatrolPoint();
                SetDestination();
            }
        }
	}

    private void SetDestination() {
        if (m_currentPatrolIndex < 0 && m_destination != null) {
            Vector3 targetVector = m_destination.transform.position;
            m_navMeshAgent.SetDestination(targetVector);
        }
        else if (m_patrolPoints != null) {
            Vector3 targetVector = m_patrolPoints[m_currentPatrolIndex].transform.position;
            m_navMeshAgent.SetDestination(targetVector);
            m_travelling = true;
        }
    }

    private void ChangePatrolPoint() {
        if (UnityEngine.Random.Range(0f, 1f) <= m_switchProbability)
            m_patrolFoward = !m_patrolFoward;

        if (m_patrolFoward)
            m_currentPatrolIndex = (m_currentPatrolIndex + 1) % m_patrolPoints.Count;
        else {
            if (--m_currentPatrolIndex < 0)
                m_currentPatrolIndex = m_patrolPoints.Count - 1;
        }

    }
}
