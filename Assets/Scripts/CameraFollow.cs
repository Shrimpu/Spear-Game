using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objectToFollow;

    public float maxDistanceX = 2.5f, maxDistanceZ = 2f;

    private void LateUpdate()
    {
        float xDifference = Mathf.Abs(objectToFollow.position.x - transform.position.x);
        float zDifference = Mathf.Abs(objectToFollow.position.z - transform.position.z);
        if (xDifference > maxDistanceX)
        {
            Vector3 destination = new Vector3(objectToFollow.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, destination, xDifference - maxDistanceX);
        }
        if (zDifference > maxDistanceZ)
        {
            Vector3 destination = new Vector3(transform.position.x, transform.position.y, objectToFollow.position.z);
            transform.position = Vector3.MoveTowards(transform.position, destination, zDifference - maxDistanceZ);
        }
    }
}
