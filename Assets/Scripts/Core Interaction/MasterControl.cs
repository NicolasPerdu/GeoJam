using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class MasterControl : MonoBehaviour
{
    public Flowchart narrative;
    static public MasterControl main;
    [HideInInspector]public PlayerAvatar[] characterList;
    [HideInInspector]public PlayerAvatar activeCharacter = null;

    bool disableInputEveryFrame = false;

    void Awake()
    {
        main = this;
        characterList = new PlayerAvatar[3];
    }

    void Start()
    {
        //narrative.ExecuteBlock("Intro");
    }
    void Update()
    {
        if (Input.GetButtonDown("Character 1") && characterList[0] != null)
            SwitchPlayer(characterList[0]);
        else if (Input.GetButtonDown("Character 2") && characterList[1] != null)
            SwitchPlayer(characterList[1]);
        else if (Input.GetButtonDown("Character 3") && characterList[2] != null)
            SwitchPlayer(characterList[2]);

        if (disableInputEveryFrame)
            activeCharacter = null;
    }

    public void SwitchPlayer(PlayerAvatar avatar)
    {
        activeCharacter = avatar;
        CamFollower.main.xLockToTarget = false;
        CamFollower.main.followObject = avatar.transform;
        
        // TODO: transition
    }

    public void DisablePlayer()
    {
        activeCharacter = null;
        disableInputEveryFrame = true;
    }
    public void EnablePlayer(int characterDimension)
    {
        disableInputEveryFrame = false;
        SwitchPlayer(characterList[Mathf.Clamp(characterDimension, 1, 3) - 1]);
    }
    public void SwitchPlayerIchi() => SwitchPlayer(characterList[0]);
    public void SwitchPlayerFuta() => SwitchPlayer(characterList[1]);
    public void SwitchPlayerSan() => SwitchPlayer(characterList[2]);
    
}
