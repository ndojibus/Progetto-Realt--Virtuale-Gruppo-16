using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyChase : MummyBaseFSM
{
    [SerializeField]
    float m_timeoutTime = 3f;

    [SerializeField]
    float m_speedFactor = 2f;

    float m_actualTimer;
    float m_originalSpeed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        mummyPatrolList.gizmoColor = Color.red;        //gizmo

        m_actualTimer = m_timeoutTime;
        m_originalSpeed = navMeshAgent.speed;
        navMeshAgent.speed *= m_speedFactor;

        AudioSource mummyScream = m_mummy.GetComponent<AudioSource>();
        if (mummyScream != null)
            mummyScream.PlayOneShot(mummyScream.clip);
        else
            Debug.LogError(this.name + ": Impossible to find audio source!");
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        animator.SetFloat("PlayerDistance", Vector3.Distance(m_mummy.transform.position, m_player.transform.position) );

        if (initialized)
        {
            Vector3 targetVector = player.transform.position;
            navMeshAgent.SetDestination(targetVector);
        }

        if (!checkPlayerVisibility())
        {
            m_actualTimer -= Time.deltaTime;
            if (m_actualTimer <= 0f)
                animator.SetBool("isChasing", false);
        }
        else
            m_actualTimer = m_timeoutTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent.speed = m_originalSpeed;
    }
}

