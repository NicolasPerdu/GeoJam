using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class San : MonoBehaviour {

    private Vector3 propelDash = Vector3.zero;

    [SerializeField] private float asplodeDelay = 3f;
    [SerializeField] private float triggerDelay = .05f;
    [SerializeField] private float propelPower = 1f;
    [SerializeField] private GameObject kaboomPrefab;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private float aslposionTime;
    [SerializeField] private SpriteRenderer sanSprite;
    private float igniteTime;
    private bool asploding = false;
    PlayerController controllerReference = null;

    private void Awake() {
        controllerReference = GetComponentInParent<PlayerController>();
    }

    private void Update() {
        if (controllerReference.isActivePlayer && Input.GetButtonDown("Action")) {
            if (asploding)
                Asplode();
            else
            {
                asploding = true;
                igniteTime = Time.timeSinceLevelLoad;
                controllerReference.PauseMovement(3);
                controllerReference.PauseJumping(3);
            }
        }

        propelDash *= .985F;
        if (propelDash.magnitude < .5F)
            propelDash = Vector3.zero;
        if ((!controllerReference.ColDown && propelDash.y <= 0) || (!controllerReference.ColUp && propelDash.y >= 0))
            transform.parent.position += propelDash * Time.deltaTime;
    }

    private void FixedUpdate() {
        if (asploding && Time.timeSinceLevelLoad - igniteTime >= asplodeDelay) {
            Asplode();
        }
    }

    private void Asplode()
    {
        Instantiate(kaboomPrefab, transform.position, Quaternion.identity);
        sanSprite.enabled = false;
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
                sanSprite.enabled = true;
            }
            break;
        }
        yield return null;
    }
}
