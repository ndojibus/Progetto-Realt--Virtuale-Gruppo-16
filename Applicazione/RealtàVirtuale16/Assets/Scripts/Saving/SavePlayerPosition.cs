using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayerPosition : PersistentData
{
    struct initialValues {
        public ulong initialX;
        public ulong initialY;
        public ulong initialZ;
        public ulong initialR;
    };

    initialValues initVal;

    [SerializeField]
    private bool m_saveOrigin = false;

    void Awake() {
        base.Awake();
    }

	void Start () {
        initVal.initialX = (ulong)(this.transform.position.x + 1000f) * 100;
        initVal.initialY = (ulong)((0.05f + 1000f) * 100f);
        initVal.initialZ = (ulong)(this.transform.position.z + 1000f) * 100;
        float angleToLong = (this.transform.rotation.eulerAngles.y + 180f) * 100f;
        initVal.initialR = (ulong) angleToLong;

        createData(initVal.initialX);      //index 0, x posizione iniziale
        createData(initVal.initialY);      //index 1, y posizione iniziale
        createData(initVal.initialZ);      //index 2, z posizione iniziale

        
        createData(initVal.initialR);                              //index 3,   rotazione iniziale
    }
	
	// Update is called once per frame
	public void savePlayerPosition () {
        if (!m_saveOrigin)
        {
            saveData(0, (ulong)(this.transform.position.x + 1000f) * 100);
            //saveData(1, (ulong)(this.transform.position.y + 1000f) * 100);
            saveData(2, (ulong)(this.transform.position.z + 1000f) * 100);

            float angleToLong = (this.transform.rotation.eulerAngles.y + 180f) * 100f;
            saveData(3, (ulong)angleToLong);
        }
    }

    public override bool loadData(int t_key, ulong t_data)
    {
        bool find = base.loadData(t_key, t_data);
        if ((int)t_data >= 0 && find)
        {
            int inventoryNumber = t_key - (objectKey * 10);
            if (inventoryNumber == 0)
            {
                float newX = (float)(t_data * 0.01f - 1000f);
                this.transform.SetPositionAndRotation(new Vector3(newX, this. transform.position.y, this.transform.position.z), this.transform.rotation);
            }
            else if (inventoryNumber == 1)
            {
                float newY = (float)(t_data * 0.01f - 1000f);
                this.transform.SetPositionAndRotation(new Vector3(this.transform.position.x, newY, this.transform.position.z), this.transform.rotation);
            }
            else if (inventoryNumber == 2) {
                float newZ= (float)(t_data * 0.01f - 1000f);
                this.transform.SetPositionAndRotation(new Vector3(this.transform.position.x, this.transform.position.y, newZ), this.transform.rotation);
            }
            else if (inventoryNumber == 3)
            {
                float newRotation = (float)(t_data * 0.01 - 180);
                this.transform.RotateAround(this.transform.position, this.transform.up, newRotation);
            }
        }
        return find;
    }
}
