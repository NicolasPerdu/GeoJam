using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollow : MonoBehaviour
{
    // the default target to move the camera to on frame 1.
    public Transform defaultTarget = null;

    // camera positional offset 
    public Vector3 cameraOffset = Vector3.zero;


    // the active target to move the camera to
    private Transform activeTarget = null;


    void Start()
    {
        activeTarget = defaultTarget;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = activeTarget.position;
    }
}
