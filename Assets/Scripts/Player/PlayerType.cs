using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerType : MonoBehaviour
{
    public TarodevController.PlayerController controller {get; private set;}
    public PlayerAvatar avatar {get; private set;}

    [HideInInspector]public Vector3 propel = Vector3.zero;

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
        
        //transform.root.position += propel * MasterControl.TimeRelator;
    }
}
