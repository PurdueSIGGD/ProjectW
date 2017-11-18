using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_TimeWallSpawner : Ability_ObjectSpawner{
    public float wallLifetime = 3;
    public float returnSpeed = 10;

    private static string GET_ROTATION_ANGLE_METHOD_NAME = "GetRotationAngle";
    private ArrayList hitByThese;
    private ArrayList velocities;
    private PlayerComponent.Buf recData;

    public override void ObjectSpawner_Start()
    {
        this.ResgisterDelegate(GET_ROTATION_ANGLE_METHOD_NAME, Callback_GetRotationAngle);

        hitByThese = new ArrayList();
        velocities = new ArrayList();
    }
    public override void OnSpellSpawned(GameObject spawn)
    {
        TimeWall t;
        if (t = spawn.GetComponent<TimeWall>())
        {
            t.StartTimeWall(this, this.wallLifetime);
        }
    }
    public void TimeWallFinished(ArrayList hitByThese, ArrayList velocities) 
    {
        this.hitByThese = hitByThese;
        this.velocities = velocities;
        if (myBase.isLocalPlayer || (myBase.myInput.isBot() && myBase.isServer))
        {
            Vector3 localAngle = aimAngle.position + aimAngle.forward * 100;
            RaycastHit[] hits = Physics.RaycastAll(aimAngle.position, aimAngle.forward * 100);
            Debug.DrawRay(aimAngle.position, aimAngle.forward * 100, Color.green, 10);
            foreach (RaycastHit h in hits)
            {
                PlayerStats tmpSts;
                if (tmpSts = h.transform.GetComponentInParent<PlayerStats>())
                {
                    if (tmpSts.gameObject == this.gameObject)
                        continue;
                }
                if (h.transform.GetComponent<Collider>().isTrigger)
                {
                    continue;
                }
                //print ("overriding with object: " + h.transform);
                localAngle = h.point;
                break;
            }


            Buf buf = new Buf();
            buf.methodName = GET_ROTATION_ANGLE_METHOD_NAME;
            buf.vectorList = new Vector3[] { localAngle };
            this.NotifyAllClientDelegates(buf);
        }

        if (recData.vectorList != null && recData.vectorList.Length > 0)
        {
            ProcessData();
        }
       
    }
    private void Callback_GetRotationAngle(PlayerComponent.Buf data)
    {
        recData = data;
        if (hitByThese != null && hitByThese.Count > 0)
        {
            ProcessData();
        }
    } 

    private void ProcessData()
    {

        Vector3 target = recData.vectorList[0];
        print("Time wall finished" + " " + target);
        for (int i = 0; i < hitByThese.Count; i++)
        {
            print("hit by" + hitByThese[i]);
            Rigidbody r = (Rigidbody)hitByThese[i];
            if (r != null && r.gameObject != null)
            {
                float mag = Vector3.Magnitude((Vector3)velocities[i]);
                Vector3 newDirection = Vector3.Normalize(target - r.transform.position);
                print(newDirection);
                r.AddForce(mag * newDirection * returnSpeed);

            }
        }
        hitByThese = null;
        velocities = null;
        recData.vectorList = null;
    }
    
}
