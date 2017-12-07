using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MummyPatrol : MummyBaseFSM
{
    [SerializeField]
    bool m_patrolWaiting = false;

    [SerializeField]
    float m_totalWaitTime = 3f;

    [SerializeField]
    float m_switchProbability = 0.2f;

    int m_currentPatrolIndex;
    bool m_travelling;
    bool m_waiting;
    bool m_patrolFoward = true;
    float m_waitTimer;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (initialized)
        {
            mummyPatrolList.gizmoColor = Color.cyan;    //gizmo

            int i = 0, nearestIndex = -1;
            float minDistance = -1.0f;
            foreach (Waypoint p in patrolPoints) {
                float distance = Vector3.Distance(p.transform.position, mummy.transform.position);
                if ((distance < minDistance) || (minDistance < 0.0f))
                {
                    nearestIndex = i;
                    minDistance = distance;
                }
                ++i;
            }


            m_currentPatrolIndex = nearestIndex;
            float nextDistance = Vector3.Distance(patrolPoints[(m_currentPatrolIndex + 1) % patrolPoints.Count].transform.position, mummy.transform.position);
            float previousDistance = Vector3.Distance(patrolPoints[(m_currentPatrolIndex + patrolPoints.Count - 1) % patrolPoints.Count].transform.position, mummy.transform.position);
            if ( nextDistance < previousDistance)
                m_patrolFoward = false;
            SetDestination();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (m_travelling && navMeshAgent.remainingDistance <= 1.0f)
        {
            m_travelling = false;

            if (m_patrolWaiting)
            {
                m_waiting = true;
                m_waitTimer = 0f;
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }

        if (m_waiting)
        {
            m_waitTimer += Time.deltaTime;
            if (m_waitTimer >= m_totalWaitTime)
            {
                m_waiting = false;

                ChangePatrolPoint();
                SetDestination();
            }
        }

        if (checkPlayerVisibility())
            animator.SetBool("isChasing", true);

    }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}
    private void SetDestination()
    {
        if (patrolPoints != null) {
            Vector3 targetVector = patrolPoints[m_currentPatrolIndex].transform.position;
            navMeshAgent.SetDestination(targetVector);
            m_travelling = true;
        }
    }

    private void ChangePatrolPoint() {
        if (UnityEngine.Random.Range(0f, 1f) <= m_switchProbability)
            m_patrolFoward = !m_patrolFoward;

        if (m_patrolFoward)
            m_currentPatrolIndex = (m_currentPatrolIndex + 1) % patrolPoints.Count;
        else {
            if (--m_currentPatrolIndex < 0)
                m_currentPatrolIndex = patrolPoints.Count - 1;
        }

    }
}
