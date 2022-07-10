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
        Instantiate(kaboomPrefab, transform.position, Quaternion.identity);
        sanVisibleObjectGroup.SetActive(false);
        controller.enabled = false;
        asploding = false;
        StartCoroutine("Asplosion");
    }

    IEnumerator Asplosion()
    {
        yield return new WaitForSeconds(aslposionTime);
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
