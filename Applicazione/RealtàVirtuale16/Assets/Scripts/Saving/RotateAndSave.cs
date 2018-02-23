using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndSave : PersistentData
{
    [SerializeField]
    private bool m_alreadySaved = false;

    LaserBehaviour m_laser;

    void Awake() {
        base.Awake();

        m_laser = GetComponentInChildren<LaserBehaviour>();
        if (m_laser == null)
            Debug.LogError(this.name + ": " + "Impossible to find LaserBehaviour!");

    }

    void Start () {
        createData(0);
    }
	
	// Update is called once per frame
    public override bool loadData(int t_key, ulong t_data)
    {
        bool find = base.loadData(t_key, t_data);
        if ((int)t_data == 1 && find)
        {
            m_alreadySaved = true;
        }
        else if ((int)t_data == 0 && find)
        {
            m_alreadySaved = false;
            m_laser.transform.RotateAround(m_laser.transform.position, m_laser.transform.up, -136.73f);
            m_laser.transform.parent.gameObject.GetComponent<InteractableLaser>().saveRotation();
            saveData(0, 1);
        }
        return find;
    }
}
