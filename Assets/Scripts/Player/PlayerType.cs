using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerType : MonoBehaviour
{
    public TarodevController.PlayerController controller {get; private set;}
    public PlayerAvatar avatar {get; private set;}

    void Awake()
    {
        controller = GetComponentInParent<TarodevController.PlayerController>();
        avatar = transform.root.GetComponentInChildren<PlayerAvatar>();
    }
}
