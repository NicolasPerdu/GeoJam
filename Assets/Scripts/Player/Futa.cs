using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class Futa : PlayerType {

    [SerializeField] private float dashDelay = 1f;
    [SerializeField] private float propelPower = 1f;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private float suspendAirTime = .35F;

    private bool dashing;
    private float? lastDash;
    private bool canDash = true;

    override protected void Update() {
        if (controller.isActivePlayer && canDash && !controller.Grounded && Input.GetButtonDown("Action") && (lastDash == null || Time.timeSinceLevelLoad - lastDash >= dashDelay)) {
            lastDash = Time.timeSinceLevelLoad;
            canDash = false;
            dashing = true;
            controller.PauseGravity(suspendAirTime);
        }

        if (dashing && lastDash == null || (lastDash != null && Time.timeSinceLevelLoad - lastDash >= suspendAirTime))
            Propel();

        Vector3 newpropel = propel * .98F;
        propel = (newpropel - propel) * MasterControl.TimeRelator;



        if (propel.magnitude < .01F)
            propel = Vector3.zero;
        
        if (controller.Grounded)
            canDash = true;

        base.Update();
    }

    private void Propel() {
        lastDash = null;
        propel = Mathf.Sign(Input.GetAxis("Vertical")) * propelPower * Vector3.up * 2.4F;
        if (propel.y < 0)
            propel.y *= .74F;
    }
}
