using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public float moveSpeed = 0.5f;

    void Update()
    {
        transform.Translate(SetDirection());
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
            dir += (Vector3.up * Input.GetAxisRaw("Vertical") * Time.deltaTime);
        }
        return dir.normalized * moveSpeed;
    }
}
