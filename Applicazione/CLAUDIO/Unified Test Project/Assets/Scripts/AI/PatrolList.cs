using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolList : MonoBehaviour
{
    [SerializeField]
    List<Waypoint> m_patrolPoints;

    float m_rayDistance = 10f;
    Color m_gizmoColor = Color.cyan;


    public List<Waypoint> patrolPoints { get { return m_patrolPoints; } }
    public float rayDistance { set { m_rayDistance = value; } }
    public Color gizmoColor { set { m_gizmoColor = value; } }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = m_gizmoColor;
        Gizmos.DrawRay(this.transform.position, this.transform.forward * m_rayDistance);
    }
}
