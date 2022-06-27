using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionAscension : MonoBehaviour
{
    [SerializeField] public GameObject ichi;
    [SerializeField] public GameObject Futa;
    [SerializeField] public GameObject San;
    [SerializeField] public Checkpoint nextDimensionCheckpoint;
    [SerializeField] public MasterControl theMaster;

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
            ichi.SetActive(true);
            Futa.SetActive(true);
            San.SetActive(true);
            nextDimensionCheckpoint.Respawn();
            theMaster.SwitchPlayerIchi();
        }
    }
}
