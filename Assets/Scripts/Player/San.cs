using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class San : PlayerType {

    private Vector3 propelDash = Vector3.zero;

    [SerializeField] private float asplodeDelay = 3f;
    [SerializeField] private float triggerDelay = .05f;
    [SerializeField] private float propelPower = 1f;
    [SerializeField] private GameObject kaboomPrefab;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private float aslposionTime;
    private GameObject sanVisibleObjectGroup;
    private float igniteTime;
    private bool asploding = false;


    void Start() {
        sanVisibleObjectGroup = transform.Find("X-Flipper").gameObject;
    }

    override protected void Update() {
        if (controller.isActivePlayer && Input.GetButtonDown("Action")) {
            if (asploding)
                Asplode();
            else
            {
                asploding = true;
                igniteTime = Time.timeSinceLevelLoad;
                controller.PauseMovement(3);
                controller.PauseJumping(3);
            }
        }

        propelDash *= .985F;
        if (propelDash.magnitude < .5F)
            propelDash = Vector3.zero;
        if ((!controller.ColDown && propelDash.y <= 0) || (!controller.ColUp && propelDash.y >= 0))
            transform.parent.position += propelDash * Time.deltaTime;

        base.Update();
    }

    void FixedUpdate() {
        if (asploding && Time.timeSinceLevelLoad - igniteTime >= asplodeDelay) {
            Asplode();
        }
    }

    private void Asplode()
    {        

        controller.enabled = false;
        asploding = false;

        StartCoroutine("Asplosion");
    }

    IEnumerator Asplosion()
    {

        Instantiate(kaboomPrefab, transform.position, Quaternion.identity);
        float delayForBooms = .1F;
        int numOfBooms = 5; 

        for (int i = 0; i < numOfBooms; i++)
        {
            Vector3 asplosionPos = new Vector3(Random.value - .5F, Random.value - .5F, Random.value - .5F) * Random.Range(1.1F, 2.75F);
            GameObject kaboom = Instantiate(kaboomPrefab, transform.position + asplosionPos, Quaternion.identity);
            kaboom.transform.localScale = Vector3.one * Random.Range(.5F, .9F);
            yield return new WaitForSeconds(delayForBooms);
        }

        sanVisibleObjectGroup.SetActive(false);


        yield return new WaitForSeconds(aslposionTime - numOfBooms * delayForBooms);
        foreach (Checkpoint checkpoint in FindObjectsOfType<Checkpoint>())
        {
            if (checkpoint.Active)
            {
                checkpoint.RespawnSan();
                sanVisibleObjectGroup.SetActive(true);
                controller.enabled = true;
            }
            break;
        }
        yield return null;
    }
}
