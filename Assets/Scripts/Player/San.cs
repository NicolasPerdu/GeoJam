using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class San : MonoBehaviour {

    private Vector3 propelDash = Vector3.zero;

    [SerializeField] private float asplodeDelay = 3.0F;
    [SerializeField] private float triggerDelay = .05f;
    [SerializeField] private float propelPower = 1f;
    [SerializeField] private GameObject kaboomPrefab;
    [SerializeField] private PlayerAnimator playerAnimator;

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
                controllerReference.PauseMovement(10);
                controllerReference.PauseJumping(10);
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

    private void Asplode() {
        Instantiate(kaboomPrefab, transform.position, Quaternion.identity);
        Destroy(transform.root.gameObject);
    }
}
