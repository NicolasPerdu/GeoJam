using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asplosion : MonoBehaviour
{
    public float maxSize = 8;
    public float expandRate = 1.1F;

    MeshRenderer meshRen;

    void Start()
    {
        meshRen = GetComponent<MeshRenderer>();
        //meshRen.material = new Material(meshRen.material);
    }

    void Update()
    {
        Vector3 newscale = transform.localScale * expandRate;

        transform.localScale += (newscale - transform.localScale) * MasterControl.TimeRelator;


        Color color = meshRen.material.color;
        color.a = (maxSize - transform.localScale.magnitude) / (maxSize / 2);
        meshRen.material.color = color;

        if (transform.localScale.magnitude > maxSize)
            Destroy(gameObject);
    }

    void OnCollisionStay(Collision c)
    {
        if (c.gameObject.tag == "Breakable")
            Destroy(c.gameObject);
    }
}
