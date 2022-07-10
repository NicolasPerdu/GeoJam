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
    [SerializeField] private KeyCode shootingButton;
    [SerializeField] private Projectile ProjectilePrefab; 
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private float triggerStunTime = .35F;

    private bool shooting;
    private float lastShot;
    
    private float? triggerTime = null;

    [SerializeField]
    private float smoothTime = 0.2F;

    private Vector3 velocity = Vector3.zero;
    private Vector3 knockback = Vector3.zero;

    PlayerController controllerReference = null;

    private void Awake() {
        controllerReference = GetComponentInParent<PlayerController>();
    }

    private void Update() {
        if (controllerReference.isActivePlayer && Input.GetButtonDown("Action") && (Time.timeSinceLevelLoad - lastShot >= shootDelay)) {
            triggerTime = Time.timeSinceLevelLoad;
        }

        if (triggerTime != null && Time.timeSinceLevelLoad - triggerTime >= triggerDelay)
        {
            shooting = true;
            triggerTime = null;
        }

        if (knockback != Vector3.zero) {
            transform.root.position = Vector3.SmoothDamp(transform.root.position, knockback, ref velocity, smoothTime);

            if ((transform.root.position.x - knockback.x) < 0.1f) {
                knockback = Vector3.zero;
            }
        }
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
        projectile.Speed = playerAnimator.transform.localScale.x * firingPower;

        shooting = false;
        lastShot = Time.timeSinceLevelLoad;
        
        controllerReference.PauseMovement(triggerStunTime);

        knockback = new Vector3(-Mathf.Sign(playerAnimator.transform.localScale.x) * 5 + transform.root.position.x, 0, 0);
    }
}
