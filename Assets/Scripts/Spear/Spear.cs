using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Spear : MonoBehaviour
{
    public SpearStats stats;

    protected Rigidbody rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerSpear>().PickUp(this);
        }
        else if (!rb.isKinematic && !other.CompareTag("Button"))
        {
            transform.parent = other.transform;
            rb.isKinematic = true;
        }
    }
}
