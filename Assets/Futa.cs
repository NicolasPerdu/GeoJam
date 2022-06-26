using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class Futa : MonoBehaviour {

    private Vector3 propelDash = Vector3.zero;

    [SerializeField] private float dashDelay = 1f;
    [SerializeField] private float propelPower = 1f;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private float suspendAirTime = .35F;

    private bool dashing;
    private float? lastDash;
    private bool canDash = true;

    PlayerController controllerReference = null;

    private void Awake() {
        controllerReference = GetComponentInParent<PlayerController>();
    }

    private void Update() {
        if (canDash && Input.GetButtonDown("Action") && (lastDash == null || Time.timeSinceLevelLoad - lastDash >= dashDelay)) {
            lastDash = Time.timeSinceLevelLoad;
            canDash = false;
            dashing = true;
            controllerReference.PauseGravity(suspendAirTime);
        }

        propelDash *= .985F;
        if (propelDash.magnitude < .5F)
            propelDash = Vector3.zero;
        if ((!controllerReference.ColDown && propelDash.y <= 0) || (!controllerReference.ColUp && propelDash.y >= 0))
            transform.parent.position += propelDash * Time.deltaTime;
        
        if (controllerReference.Grounded)
            canDash = true;
    }

    private void FixedUpdate() {
        if (dashing && lastDash != null && Time.timeSinceLevelLoad - lastDash >= suspendAirTime){
            Propel();
        }
    }

    private void Propel() {
        lastDash = null;
        propelDash = Mathf.Sign(Input.GetAxis("Vertical")) * propelPower * Vector3.up * 100;
    }
}
