using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.velocity = SetDirection();
    }

    Vector3 SetDirection()
    {
        Vector3 dir = new Vector3();
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            dir += (Vector3.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime);
        }
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            dir += (Vector3.forward * Input.GetAxisRaw("Vertical") * Time.deltaTime);
        }
        return dir.normalized * moveSpeed;
    }
}
