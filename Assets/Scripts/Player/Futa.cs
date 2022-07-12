using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class Futa : PlayerType {

    [SerializeField] private float dashDelay = 1f;
    [SerializeField] private float propelPower = 1f;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private float suspendAirTime = .35F;
    [SerializeField] private ParticleSystem _buildupParticles;

    private bool dashing;
    private float? lastDash;
    private bool canDash = true;

    int propelDir = 0;

    Vector3 lastPos;

    void Start()
    {
        lastPos = transform.root.position;
    }

    override protected void Update() {
        Vector3 movementLastFrame = transform.root.position - lastPos;


        if (controller.isActivePlayer && canDash && !controller.Grounded && Input.GetButtonDown("Action") && (lastDash == null || Time.timeSinceLevelLoad - lastDash >= dashDelay)) {
            lastDash = Time.timeSinceLevelLoad;
            canDash = false;
            dashing = true;
            controller.PauseGravity(suspendAirTime);
            _buildupParticles.Play();
        }

        if (dashing)
        {     
            if (lastDash != null && Time.timeSinceLevelLoad - lastDash >= suspendAirTime)
                Propel();
            if (propelDir > 0 && movementLastFrame.y < 0)
                playerAnimator._moveParticles.Stop();
        }

        Vector3 newpropel = propel * .98F;
        propel += (newpropel - propel) * MasterControl.TimeRelator;


        if (controller.Grounded)
        {
            canDash = true;
            propel.y = 0;
            propelDir = 0;
            dashing = false;
        }

        base.Update();


        lastPos = transform.root.position;
    }

    private void Propel() {
        _buildupParticles.Stop();
        playerAnimator._moveParticles.Play();
        lastDash = null;
        propel = Mathf.Sign(Input.GetAxis("Vertical")) * propelPower * Vector3.up * 0.4F;
        propelDir = propel.y > 0 ? 1 : -1;
        if (propel.y < 0)
            propel.y *= .74F;
    }
}
