using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParent : MonoBehaviour
{
    void LateUpdate()
    {
        if (transform.parent != null)
        {
            transform.rotation = transform.parent.rotation;
            transform.position = transform.parent.position;
        }
    }
}
