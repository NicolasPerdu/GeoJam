using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    private void Awake() {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
