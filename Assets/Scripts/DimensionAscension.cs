using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class DimensionAscension : MonoBehaviour
{
    [SerializeField] public GameObject ichi;
    [SerializeField] public GameObject Mobius;
    [SerializeField] public GameObject Gordian;
    [SerializeField] public Checkpoint nextDimensionCheckpoint;
    [SerializeField] public int destinationDimension;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (destinationDimension == 2)
            {
                ichi.SetActive(true);
                Mobius.SetActive(true);
                ichi.GetComponent<PlayerController>().EnableSelf();
                Mobius.GetComponent<PlayerController>().EnableSelf();
                nextDimensionCheckpoint.Respawn(FindObjectsOfType<PlayerType>());
                MasterControl.main.SwitchPlayerIota();
                MasterControl.main.avatarList[1].playable = true;
            }
            else
            {
                ichi.SetActive(true);
                Mobius.SetActive(true);
                Gordian.SetActive(true);
                ichi.GetComponent<PlayerController>().EnableSelf();
                Mobius.GetComponent<PlayerController>().EnableSelf();
                Gordian.GetComponent<PlayerController>().EnableSelf();
                nextDimensionCheckpoint.Respawn(FindObjectsOfType<PlayerType>());
                MasterControl.main.SwitchPlayerIota();
                MasterControl.main.avatarList[2].playable = true;
            }
        }
    }
}
