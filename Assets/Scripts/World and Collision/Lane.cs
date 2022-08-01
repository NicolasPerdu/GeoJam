using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public LayerMask groundLayer;
    public int laneIndex {get; private set;}

    void Awake()
    {
        MasterControl.main.lanes.Add(this);
        laneIndex = -1;
    }

    void Update()
    {
        if (laneIndex < 0)
            laneIndex = MasterControl.main.lanes.IndexOf(this);
    }
}
