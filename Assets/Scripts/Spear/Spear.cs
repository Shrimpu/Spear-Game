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
        if (!other.CompareTag("Player") && !other.CompareTag("Button"))
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }
            rb.isKinematic = true;
        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerSpear>().PickUp(this);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (!rb.isKinematic)
        {
            if (!other.CompareTag("Player") && !other.CompareTag("Button"))
            {
                if (rb == null)
                {
                    rb = GetComponent<Rigidbody>();
                }
                rb.isKinematic = true;
            }
        }
    }
}
