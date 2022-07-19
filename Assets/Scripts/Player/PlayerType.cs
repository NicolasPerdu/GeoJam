using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerType : MonoBehaviour
{
    public TarodevController.PlayerController controller {get; private set;}
    public PlayerAvatar avatar {get; private set;}

    [HideInInspector]public Vector3 propel = Vector3.zero;
    [HideInInspector]public int laneIndex = 0;

    private int zDir = 0;

    private float targetLaneZ;
    [HideInInspector]public bool laneSwitching = false;

    void Awake()
    {
        controller = GetComponentInParent<TarodevController.PlayerController>();
        avatar = transform.root.GetComponentInChildren<PlayerAvatar>();
    }

    virtual protected void Update()
    {    
        if ((controller.ColLeft && propel.x < 0) || (controller.ColRight && propel.x > 0))
            propel.x = 0;
        if ((controller.ColDown && propel.y < 0) || (controller.ColUp && propel.y > 0))
            propel.y = 0;
        

        Vector3 pos = transform.root.position;

        if ((zDir > 0 && transform.root.position.z >= targetLaneZ) || (zDir < 0 && transform.root.position.z <= targetLaneZ))
        {                
            pos.z = targetLaneZ;
            transform.root.position = pos;

            laneSwitching = false;
            zDir = 0;
        }

        pos = transform.root.position;
        pos.z += zDir * .9F * MasterControl.TimeRelator;
        transform.root.position = pos;
    }

    public void SetLane(int index)
    {
        if (index < 0 || index >= MasterControl.main.lanes.Count) return;
        
        zDir = index - laneIndex;

        Lane lane = MasterControl.main.lanes[index];
        targetLaneZ = lane.transform.position.z;
        laneIndex = index;
        laneSwitching = true;
    }

    public void MoveLane(int dir) => SetLane(laneIndex + dir);
    public void MoveLane(float dir) => MoveLane((int)dir);

}
