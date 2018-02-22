using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyAttack : MummyBaseFSM
{
    [SerializeField]
    private float m_playerKillDistance;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        mummyPatrolList.gizmoColor = Color.red;        //gizmo
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (stateInfo.normalizedTime >= 0.9)
        {
            float currentDistance = Vector3.Distance(m_mummy.transform.position, m_player.transform.position);
            if (currentDistance < m_playerKillDistance)
                Debug.Log("PLAYER IS DEAD");
            else
                animator.SetFloat("PlayerDistance", currentDistance);
        }
	}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
