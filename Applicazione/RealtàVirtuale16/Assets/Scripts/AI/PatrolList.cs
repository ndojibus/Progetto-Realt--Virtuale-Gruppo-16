using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolList : MonoBehaviour
{
    [SerializeField]
    List<Waypoint> m_patrolPoints;

    float m_fowardRayDistance = 10f;
    Vector3 m_gizmoFowardDirection;
    float m_backwardRayDistance = 0f;
    Vector3 m_gizmoBackwardDirection;
    Color m_gizmoColor = Color.cyan;
    protected bool m_canChase = true;

    public List<Waypoint> patrolPoints { get { return m_patrolPoints; } }
    public float fowardRayDistance { set { m_fowardRayDistance = value; } }
    public Vector3 gizmoFowardDirection { set { m_gizmoFowardDirection = value; } }
    public float backwardRayDistance { set { m_backwardRayDistance = value; } }
    public Vector3 gizmoBackwardDirection { set { m_gizmoBackwardDirection = value; } }
    public Color gizmoColor { set { m_gizmoColor = value; } }
    public bool canChase { get { return m_canChase; } set { m_canChase = value; } }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = m_gizmoColor;
        Vector3 startRaycastPosition = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z);
        Gizmos.DrawRay(startRaycastPosition, m_gizmoFowardDirection * m_fowardRayDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(startRaycastPosition, m_gizmoBackwardDirection * m_backwardRayDistance);
    }
}
