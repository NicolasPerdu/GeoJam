using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterControl : MonoBehaviour
{
    static public MasterControl main;
    [HideInInspector]public PlayerAvatar[] characterList;
    [HideInInspector]public PlayerAvatar activeCharacter = null;

    void Awake()
    {
        main = this;
        characterList = new PlayerAvatar[3];
    }

    void Update()
    {
        if (Input.GetButtonDown("Character 1") && characterList[0] != null)
            SwitchPlayer(characterList[0]);
        else if (Input.GetButtonDown("Character 2") && characterList[1] != null)
            SwitchPlayer(characterList[1]);
        else if (Input.GetButtonDown("Character 3") && characterList[2] != null)
            SwitchPlayer(characterList[2]);
    }

    public void SwitchPlayer(PlayerAvatar avatar)
    {
        activeCharacter = avatar;
        CamFollower.main.xLockToTarget = false;
        CamFollower.main.followObject = avatar.transform;
        
        // TODO: transition
    }
}
