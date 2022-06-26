using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FungusTrigger : MonoBehaviour
{
    [SerializeField] public Flowchart narrative;
    [SerializeField] public float timer;
    private float timerCountdown;
    private bool hasBeenTriggeredAlready;
    // Start is called before the first frame update
    void Start()
    {
        hasBeenTriggeredAlready = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerCountdown >= 0f)
        {
            timerCountdown -= Time.deltaTime;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasBeenTriggeredAlready)
            {
                narrative.ExecuteBlock("IchiShootGunTutorial");
                hasBeenTriggeredAlready = true;
            }
            else if (timerCountdown <= 0f)
            {
                narrative.ExecuteBlock("IchiShootGunTutorial");
            }
            timerCountdown = timer;

        }
    }
}
