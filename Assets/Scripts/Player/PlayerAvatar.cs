using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    public bool playable {get; set;}
    [Range(1, 3)]public int playerCharacterNumber = 1;

    void Awake()
    {
         playable = playerCharacterNumber == 1;
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
