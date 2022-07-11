using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float Speed;

    private float spawnTime;
    private float? offScreenTime;

    private void Awake() {
        spawnTime = Time.timeSinceLevelLoad;
    }

    private void Update() {
        //transform.position = new Vector3(this.transform.position.x + Speed, transform.position.y, this.transform.position.z);
        Vector3 pos = transform.position;
        pos.x += Speed * MasterControl.TimeRelator;
        transform.position = pos;


        if (Time.timeSinceLevelLoad - spawnTime > 10 || 
        (offScreenTime != null && Time.timeSinceLevelLoad - offScreenTime > 0.5F)) {
            Destroy(gameObject); //Hardcoded high value that gameplay wise is infinite anyway
        }
    }

    void OnBecameVisible() => offScreenTime = null;

    void OnBecameInvisible() => offScreenTime = Time.timeSinceLevelLoad;
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Breakable"))
        {
            other.gameObject.SetActive(false);
        }
    }

}
