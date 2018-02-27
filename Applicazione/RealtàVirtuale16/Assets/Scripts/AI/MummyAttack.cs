using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyAttack : MummyBaseFSM
{
    [SerializeField]
    private float m_playerKillDistance;

    private bool m_playerIsDead = false;

    private UIManager m_deathScreen;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        mummyPatrolList.gizmoColor = Color.red;        //gizmo

        m_deathScreen = FindObjectOfType<UIManager>();
        if (m_deathScreen == null)
            Debug.LogError("Impossible to find death screen");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (stateInfo.normalizedTime >= 0.9)
        {
            float currentDistance = Vector3.Distance(m_mummy.transform.position, m_player.transform.position);
            if (currentDistance < m_playerKillDistance && !m_playerIsDead)
            {
                m_deathScreen.DeathScreen();
                m_playerIsDead = true;
            }
            else
                animator.SetFloat("PlayerDistance", currentDistance);
        }
	}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
