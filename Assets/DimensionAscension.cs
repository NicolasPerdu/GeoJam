using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class DimensionAscension : MonoBehaviour
{
    [SerializeField] public GameObject ichi;
    [SerializeField] public GameObject Futa;
    [SerializeField] public GameObject San;
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
                Futa.SetActive(true);
                ichi.GetComponent<PlayerController>().EnableSelf();
                Futa.GetComponent<PlayerController>().EnableSelf();
                nextDimensionCheckpoint.Respawn();
                MasterControl.main.SwitchPlayerIchi();
                MasterControl.main.avatarList[1].playable = true;
            }
            else
            {
                ichi.SetActive(true);
                Futa.SetActive(true);
                San.SetActive(true);
                ichi.GetComponent<PlayerController>().EnableSelf();
                Futa.GetComponent<PlayerController>().EnableSelf();
                San.GetComponent<PlayerController>().EnableSelf();
                nextDimensionCheckpoint.Respawn();
                MasterControl.main.SwitchPlayerIchi();
                MasterControl.main.avatarList[2].playable = true;
            }
        }
    }
}
