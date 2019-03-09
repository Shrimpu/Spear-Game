using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Spear : MonoBehaviour
{
    public SpearStats stats;

    Rigidbody rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
            rb.isKinematic = true;
        }
        else
        {
            other.GetComponent<PlayerSpear>().PickUp(this);
        }
    }
}
