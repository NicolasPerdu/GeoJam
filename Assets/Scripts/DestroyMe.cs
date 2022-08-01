using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    void OnTriggerExit(Collider c)
    {
        if (c.transform.root.tag == "Player")
        {
            Destroy(transform.root.gameObject);
        }
    }
}
