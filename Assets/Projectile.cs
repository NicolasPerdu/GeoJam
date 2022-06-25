using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float Speed;

    private float spawnTime;

    private void Awake() {
        spawnTime = Time.timeSinceLevelLoad;
    }

    private void FixedUpdate() {
        transform.position = new Vector3(this.transform.position.x + Speed, this.transform.position.y, this.transform.position.z);


        if (Time.timeSinceLevelLoad - spawnTime > 20) {
            Destroy(gameObject); //Hardcoded high value that gameplay wise is infinite anyway
        }
    }
    
    
}
