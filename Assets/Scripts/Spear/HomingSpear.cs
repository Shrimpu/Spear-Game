using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LookAtMouse))]
public class HomingSpear : Spear
{
    public float floatTime = 2f;
    float timer;
    float moveSpeed;
    bool toggleOn;
    LookAtMouse lookAtMouse;
    Transform player;
    CameraFollow camera;

    protected override void Start()
    {
        base.Start();
        rb.useGravity = false;
        lookAtMouse = GetComponent<LookAtMouse>();
        lookAtMouse.look = false;
        camera = FindObjectOfType<CameraFollow>();
        player = camera.objectToFollow;
    }

    private void Update()
    {
        if (!rb.isKinematic)
        {
            if (!toggleOn)
            {
                timer = Time.time + floatTime;
                lookAtMouse.look = true;
                moveSpeed = rb.velocity.magnitude;
                toggleOn = true;
                camera.objectToFollow = transform;
            }
            if (timer < Time.time)
                rb.useGravity = true;
            else
            {
                rb.velocity = transform.forward * moveSpeed;
            }
        }
        else if (toggleOn)
        {
            lookAtMouse.look = false;
            rb.useGravity = false;
            camera.objectToFollow = null;
            camera.relocate = true;
            camera.objectToFollow = player;
            toggleOn = false;
        }
    }
}
