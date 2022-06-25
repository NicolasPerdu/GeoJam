using System;
using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class Ichi : MonoBehaviour
{
    [SerializeField] private Transform projectileSpawnpoint;
    [SerializeField] private float shootDelay = 1f;
    [SerializeField] private float firingPower = 1f;
    [SerializeField] private KeyCode shootingButton;
    [SerializeField] private Projectile ProjectilePrefab; 

    [SerializeField] private PlayerAnimator playerAnimator;

    private bool shooting;
    private float lastShot;

    private void Update() {
        if (Input.GetKey(shootingButton) && (Time.timeSinceLevelLoad - lastShot >= shootDelay)) {
            shooting = true;
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
    }
}
