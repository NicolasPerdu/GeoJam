using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    [Range(1, 3)]public int playerCharacterNumber = 1;
    void Start()
    {
        if (MasterControl.main.characterList[playerCharacterNumber - 1] == null)
        {
            MasterControl.main.characterList[playerCharacterNumber - 1] = this;
            MasterControl.main.SwitchPlayer(this);  // temporary
        }
    }
}
