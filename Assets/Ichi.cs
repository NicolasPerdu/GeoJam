using System;
using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class Ichi : MonoBehaviour
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

    private Vector3 knockback = Vector3.zero;

    PlayerController controllerReference = null;

    private void Awake() {
        controllerReference = GetComponentInParent<PlayerController>();
    }

    private void Update() {
        if (Input.GetButtonDown("Action") && (Time.timeSinceLevelLoad - lastShot >= shootDelay)) {
            triggerTime = Time.timeSinceLevelLoad;
        }

        if (triggerTime != null && Time.timeSinceLevelLoad - triggerTime >= triggerDelay)
        {
            shooting = true;
            triggerTime = null;
        }

        knockback *= .98F;
        if (knockback.magnitude < .5)
            knockback = Vector3.zero;
        transform.parent.position += knockback * Time.deltaTime;

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
        knockback = controllerReference.fakingDirection * Vector3.left * 90 + Vector3.up * 15;
        Debug.Log(knockback);
    }
}
