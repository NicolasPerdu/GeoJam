using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    static public CamFollower main;
    const float MAX_FOLLOW_DIST =   4.0F;
    const float MIN_FOLLOW_DIST =   0.1F;
    const float FOLLOW_LAG      =   0.992F;
    const float CATCHUP_LAG     =   FOLLOW_LAG * FOLLOW_LAG;


    [HideInInspector]public bool xLockToTarget = false;
    float followFactor = FOLLOW_LAG;


    [HideInInspector]public Transform followObject = null;

    void Awake()
    {
        main = this;
    }


    void Update()
    {
        if (followObject != null)
            CameraChaseObject();
        else if (MasterControl.main.activeCharacter != null)
            followObject = MasterControl.main.activeCharacter.transform;
    }

    void CameraChaseObject()
    {
        Vector3 currentPos = transform.position;
        Vector3 targetPos = followObject.transform.position;

        if (xLockToTarget)
            currentPos.x = targetPos.x;

        Vector3 difference = targetPos - currentPos;
        float distance = difference.magnitude;
        Vector3 normal = difference.normalized;

        if (distance > MAX_FOLLOW_DIST)     // if falling behind, speed up to catch up to the player character position
        {
            distance = MAX_FOLLOW_DIST;
            followFactor = CATCHUP_LAG;
        }

        if (distance < MIN_FOLLOW_DIST)
        {
            //distance = 0;
            followFactor = FOLLOW_LAG;
            xLockToTarget = true;
        }


        distance *= followFactor;

        transform.position = targetPos - normal * distance;
    }

}
