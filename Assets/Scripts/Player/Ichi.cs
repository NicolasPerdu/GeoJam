using System;
using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class Ichi : PlayerType
{
    [SerializeField] private Transform projectileSpawnpoint;
    [SerializeField] private float shootDelay = 1f;
    [SerializeField] private float triggerDelay = .05f;
    [SerializeField] private float firingPower = 1f;
    [SerializeField] private float knockbackPower = 1f;
    [SerializeField] private KeyCode shootingButton;
    [SerializeField] private Projectile ProjectilePrefab; 
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private float triggerStunTime = .35F;

    private bool shooting;
    private float lastShot;
    
    private float? triggerTime = null;


    override protected void Update() {

        if (controller.isActivePlayer && Input.GetButtonDown("Action") && (Time.timeSinceLevelLoad - lastShot >= shootDelay)) {
            triggerTime = Time.timeSinceLevelLoad;
        }

        if (triggerTime != null && Time.timeSinceLevelLoad - triggerTime >= triggerDelay)
        {
            shooting = true;
            triggerTime = null;
        }

        Vector3 tempPropel = propel * .945F;

        propel += (tempPropel - propel) * MasterControl.TimeRelator;

        if (controller.Grounded)    // ground friction
            propel.x += (tempPropel * .975F - propel).x * MasterControl.TimeRelator;


        if (propel.magnitude < .05F)
            propel = Vector3.zero;

        if (propel.x < 0 && controller.ColLeft) propel.x = 0;
        if (propel.x > 0 && controller.ColRight) propel.x = 0;

        base.Update();
    }

    private void FixedUpdate() {
        if (shooting) {
            Shoot();
        }
    }

    private void Shoot()
    {
        Projectile projectile = 
        Instantiate(ProjectilePrefab, projectileSpawnpoint.position, projectileSpawnpoint.rotation).GetComponent<Projectile>();
        projectile.Speed = playerAnimator.FacingDirection * firingPower;

        shooting = false;
        lastShot = Time.timeSinceLevelLoad;
        
        controller.PauseMovement(triggerStunTime);
        propel = Mathf.Sign(playerAnimator.FacingDirection) * Vector3.left * .82F * knockbackPower + Vector3.up * .24F;
    }
}
