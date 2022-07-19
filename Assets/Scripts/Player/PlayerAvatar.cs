using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class PlayerAvatar : MonoBehaviour
{

    public bool defaultIsPlayable = false;
    public bool playable {get; set;}

    [HideInInspector]public PlayerType playerType;
    [Range(1, 3)]public int playerCharacterNumber = 1;



    void Awake()
    {
        playable = defaultIsPlayable;
        playerType = transform.root.GetComponentInChildren<PlayerType>();
    }
    
    void Start()
    {
        if (MasterControl.main.avatarList[playerCharacterNumber - 1] == null)
        {
            MasterControl.main.avatarList[playerCharacterNumber - 1] = this;
        }

        if (playerCharacterNumber == 1)
            MasterControl.main.SwitchPlayer(this);
    }
}
