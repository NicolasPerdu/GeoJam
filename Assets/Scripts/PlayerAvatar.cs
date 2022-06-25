using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    void Start()
    {
        MasterControl.main.characterList.Add(this);

        MasterControl.main.SwitchPlayer(this);  // temporary

    }
}
