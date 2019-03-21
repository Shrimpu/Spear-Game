using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpear : MonoBehaviour
{
    public delegate void HeldSpearInfo(Spear spear);
    public static HeldSpearInfo PickedUp;

    public Spear heldSpear;
    public Transform spearHolder;
    [Space]
    public float retractionDistance = 0.75f;

    float charge;
    float Force
    {
        get { return force; }
        set
        {
            force = value;
            if (heldSpear != null)
                force = Mathf.Clamp(value, heldSpear.stats.minForce, heldSpear.stats.maxForce);
        }
    }
    float force;
    Vector3 chargedPos;

    void Update()
    {
        ThrowSpear();
    }

    private void ThrowSpear()
    {
        if (heldSpear != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (Force < heldSpear.stats.minForce)
                    Force = heldSpear.stats.minForce;
                else
                {
                    float forceToAdd = ((heldSpear.stats.maxForce - heldSpear.stats.minForce) / heldSpear.stats.chargeTime) * Time.deltaTime;
                    Force += forceToAdd;
                }
                charge = (Force - heldSpear.stats.minForce) / (heldSpear.stats.maxForce - heldSpear.stats.minForce);

                chargedPos = Vector3.back * retractionDistance;
                heldSpear.transform.localPosition = Vector3.Lerp(Vector3.zero, chargedPos, charge);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Rigidbody spearRB = heldSpear.GetComponent<Rigidbody>();
                heldSpear.transform.parent.DetachChildren();
                spearRB.isKinematic = false;
                spearRB.velocity = heldSpear.transform.forward * Force;
                heldSpear = null;
                Force = 0;
            }
        }
    }

    public bool PickUp(Spear spear)
    {
        if (heldSpear == null)
        {
            heldSpear = spear;
            spear.transform.parent = spearHolder;
            spear.transform.localPosition = Vector3.zero;

            PickedUp?.Invoke(spear);
            return true;
        }
        else
            return false;
    }

    private void LateUpdate()
    {
        if (heldSpear != null)
            heldSpear.transform.rotation = spearHolder.rotation;
    }
}
