using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objectToFollow;
    public float relocationSpeed = 50f;
    [HideInInspector]
    public bool relocate;
    bool relocating;

    public float maxDistanceX = 2.5f, maxDistanceZ = 2f;

    private void LateUpdate()
    {
        if (!relocate && objectToFollow != null)
            FollowObject();
        else if (relocate && !relocating && objectToFollow != null)
        {
            relocating = true;
            StartCoroutine(MoveToTarget());
        }
    }

    void FollowObject()
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

    IEnumerator MoveToTarget()
    {
        while (transform.position != objectToFollow.position)
        {
            Vector3 destination = new Vector3(objectToFollow.position.x, transform.position.y, objectToFollow.position.z);
            transform.position = Vector3.MoveTowards(transform.position, destination, relocationSpeed * Time.deltaTime);
            yield return null;
        }
        relocating = false;
        relocate = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(transform.position.x - maxDistanceX, objectToFollow.position.y, transform.position.z - maxDistanceZ),
            new Vector3(transform.position.x - maxDistanceX, objectToFollow.position.y, transform.position.z + maxDistanceZ));
        Gizmos.DrawLine(new Vector3(transform.position.x + maxDistanceX, objectToFollow.position.y, transform.position.z - maxDistanceZ),
            new Vector3(transform.position.x + maxDistanceX, objectToFollow.position.y, transform.position.z + maxDistanceZ));
        Gizmos.DrawLine(new Vector3(transform.position.x + maxDistanceX, objectToFollow.position.y, transform.position.z + maxDistanceZ),
            new Vector3(transform.position.x - maxDistanceX, objectToFollow.position.y, transform.position.z + maxDistanceZ));
        Gizmos.DrawLine(new Vector3(transform.position.x + maxDistanceX, objectToFollow.position.y, transform.position.z - maxDistanceZ),
            new Vector3(transform.position.x - maxDistanceX, objectToFollow.position.y, transform.position.z - maxDistanceZ));
    }
}
