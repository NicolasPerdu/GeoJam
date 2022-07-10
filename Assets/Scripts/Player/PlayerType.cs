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
        transform.root.position += propel * MasterControl.TimeRelator;
    }
}
