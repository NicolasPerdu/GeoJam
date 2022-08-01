using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class MasterControl : MonoBehaviour
{
    const float FIXED_DELAY_MS = .02F;
    static public float TimeRelator => Time.deltaTime / Time.fixedDeltaTime;

    public Flowchart narrative;
    static public MasterControl main;
    [HideInInspector]public PlayerAvatar[] avatarList;
    [HideInInspector]public PlayerAvatar activeAvatar = null;
    [HideInInspector]public List<Lane> lanes;

    bool disableInputEveryFrame = false;

    void Awake()
    {
        main = this;
        avatarList = new PlayerAvatar[3];
        lanes = new List<Lane>();
    }

    void Start()
    {
        //narrative.ExecuteBlock("Intro");
        if (activeAvatar == null && avatarList.Length > 0)
            activeAvatar = avatarList[0];
        lanes.Sort((Lane a, Lane b) => Mathf.FloorToInt(a.transform.position.z - b.transform.position.z));
    }

    void Update()
    {
        int pIndex = -1;
        if (Input.GetButtonDown("Character 1"))
            pIndex = 0;
        else if (Input.GetButtonDown("Character 2"))
            pIndex = 1;
        else if (Input.GetButtonDown("Character 3"))
            pIndex = 2;

        if (pIndex >= 0)
        {
            PlayerAvatar avatarToSwitchTo = avatarList[pIndex];
            if (avatarToSwitchTo != null && avatarToSwitchTo.playable)
                SwitchPlayer(avatarToSwitchTo);
        }


        if (disableInputEveryFrame)
            activeAvatar = null;
    }

    public void SwitchPlayer(PlayerAvatar avatar)
    {
        activeAvatar = avatar;
        CamFollower.main.xLockToTarget = false;
        CamFollower.main.followObject = avatar.transform;
        
        // TODO: transition
    }

    public void DisablePlayer()
    {
        activeAvatar = null;
        disableInputEveryFrame = true;
    }
    public void EnablePlayer(int characterDimension)
    {
        disableInputEveryFrame = false;
        SwitchPlayer(avatarList[Mathf.Clamp(characterDimension, 1, 3) - 1]);
    }
    public void SwitchPlayerIchi() => SwitchPlayer(avatarList[0]);
    public void SwitchPlayerFuta() => SwitchPlayer(avatarList[1]);
    public void SwitchPlayerSan() => SwitchPlayer(avatarList[2]);
    
}
