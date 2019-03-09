using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LookAtMouse : MonoBehaviour
{
    float camRayLength = 100f;
    float planeDistanceFromCameraY = 10f;
    Vector3 planePos;
    Transform mainCam;

    Rigidbody rb;
    LayerMask groundMask;
    Plane raycastPlane;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundMask = LayerMask.GetMask("Ground");
        mainCam = Camera.main.transform;

        planePos = new Vector3(mainCam.position.x, mainCam.position.y - planeDistanceFromCameraY, mainCam.position.z);
        raycastPlane = new Plane(Vector3.up, planePos);
    }

    void LateUpdate()
    {
        Turn();
    }

    void Turn()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (raycastPlane.Raycast(camRay, out float enter))
        {
            Vector3 hitPoint = camRay.GetPoint(enter);
            Vector3 playerToMouse = hitPoint - transform.position;
            Debug.DrawLine(transform.position, hitPoint, Color.red);

            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            rb.MoveRotation(newRotation);
        }
    }
}
