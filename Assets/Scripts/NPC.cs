using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour{

    public GameObject player;
    private bool following = false;
    public float movementSpeed = 0.1f;

    public void setFollowing(bool follow) {
        following = follow;
    }

    void Start() {
        
    }

 
    void Update()
    {
        if(following) {
            Vector3 targetPos = player.transform.position - player.transform.up * 2f;

            Vector2 moveToward = Vector2.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
            transform.position = moveToward;
        }
    }
}
