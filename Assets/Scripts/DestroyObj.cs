using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
            Destroy(transform.root.gameObject);
        Debug.Log("sandwich");
    }
}
