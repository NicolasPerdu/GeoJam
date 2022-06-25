using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterControl : MonoBehaviour
{
    static public MasterControl main;
    [HideInInspector]public List<PlayerAvatar> characterList;
    [HideInInspector]public PlayerAvatar activeCharacter = null;

    void Awake()
    {
        main = this;
    }

    public void SwitchPlayer(PlayerAvatar avatar)
    {
        activeCharacter = avatar;
        CamFollower.main.xLockToTarget = false;
        Debug.Log(activeCharacter == null);
        // TODO: transition
    }
}
