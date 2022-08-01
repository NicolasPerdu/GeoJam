using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class PT_Gordian : PlayerType {

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

    private Vector3 propelOthers = Vector3.zero;

    private List<PlayerType> touchingPlayerBodies = new List<PlayerType>();


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
                propelOthers = Vector3.back + Vector3.up * propelPower;
                
                igniteTime = Time.timeSinceLevelLoad;
                controller.PauseMovement(3);
                controller.PauseJumping(3);
                controller.PauseGravity(3);
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
        if (asploding)
        {
            // if igniteTime seconds has not passed, don't asplode, and check for asplode direction
            if (Time.timeSinceLevelLoad - igniteTime < asplodeDelay) 
            {
                if (controller.isActivePlayer)
                {
                    // check for asplode direction (forward)
                    if (Input.GetAxis("Vertical") > 0)
                        propelOthers.z = -1;

                    // check for asplode direction (backward)
                    if (Input.GetAxis("Vertical") < 0)
                        propelOthers.z = 1;
                }

                Vector3 pos = transform.localPosition;
                pos.z = -propelOthers.z;
                transform.localPosition = pos;
            }
            else // if time is equal or greater to igniteTime
                Asplode();
        }
    }

    private void Asplode()
    {        
        controller.enabled = false;
        asploding = false;

        RaycastHit[] hitInfos = Physics.SphereCastAll(transform.root.position, 2.0F, Vector3.left, 3.0F, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide);

        foreach (RaycastHit hit in hitInfos)
        {   
            PlayerType playerType = hit.collider.transform.GetComponentInChildren<PlayerType>();
            if (playerType != null && playerType != this)
            {
                if (!touchingPlayerBodies.Contains(playerType))
                {
                    touchingPlayerBodies.Add(playerType);
                    playerType.controller.PauseMovement(igniteTime);
                    playerType.controller.PauseJumping(igniteTime);
                    if (controller.isActivePlayer)
                        MasterControl.main.SwitchPlayer(playerType.avatar);
                }
            }
        }

        StartCoroutine("Asplosion");
    }

    private void PropelOthers()
    {
        foreach (PlayerType playerType in touchingPlayerBodies)
        {
            playerType.propel = propelOthers;
            playerType.MoveLane((int)Mathf.Sign(propelOthers.z));
            playerType.controller.UnpauseJumping();
            playerType.controller.UnpauseMovement();
        }

        if (touchingPlayerBodies.Count == 0)
            MasterControl.main.SwitchPlayerIota();
        
        propelOthers = Vector3.zero;
        touchingPlayerBodies = new List<PlayerType>();
    }

    IEnumerator Asplosion()
    {
        Instantiate(kaboomPrefab, transform.position, Quaternion.identity);
        float delayForBooms = .035F;
        int numOfBooms = 8;

        for (int i = 0; i < numOfBooms; i++)
        {
            Vector3 asplosionPos = new Vector3(Random.value - .5F, Random.value - .5F, Random.value - .5F) * Random.Range(1.1F, 2.75F);
            GameObject kaboom = Instantiate(kaboomPrefab, transform.position + asplosionPos, Quaternion.identity);
            kaboom.transform.localScale = Vector3.one * Random.Range(.5F, .9F);
            yield return new WaitForSeconds(delayForBooms);
        }

        PropelOthers();

        sanVisibleObjectGroup.SetActive(false);


        yield return new WaitForSeconds(aslposionTime - numOfBooms * delayForBooms);
        foreach (Checkpoint checkpoint in FindObjectsOfType<Checkpoint>())
        {
            if (checkpoint.Active)
            {
                checkpoint.RespawnSan();
                Vector3 pos = transform.localPosition;
                pos.z = 0;
                transform.localPosition = pos;
                sanVisibleObjectGroup.SetActive(true);
                controller.enabled = true;
            }
            break;
        }

        yield return null;
    }
}
