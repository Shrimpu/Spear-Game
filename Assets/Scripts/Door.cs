using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Door : MonoBehaviour
{
    enum States { closed, opening, open, closing }
    States doorState = new States();

    public int inputsRequired = 1;
    [Space]
    public float openAngle = 70f;
    [Range(0.1f, 2f)]
    public float openTime = 0.5f;
    [Range(0.1f, 2f)]
    public float closeTime = 0.5f;
    float startAngle = 0f;
    float inputsActive;

    [HideInInspector]
    public bool closeDoorWhenAvailable;

    [Space]
    public bool haxMode;
    [SerializeField]
    bool openFromLeft;
    [SerializeField]
    bool startOpen;

    BoxCollider boxColl;
    Vector3 startPos;
    Vector2 hinge;

    private void Awake()
    {
        boxColl = GetComponent<BoxCollider>();
        startAngle = transform.rotation.y;
        startPos = transform.position;

        if (startOpen)
            doorState = States.open;
        else
            doorState = States.closed;
    }

    public void AddActiveInput()
    {
        inputsActive++;
        if (inputsActive == inputsRequired)
            ToggleDoor();
    }

    public void SubtractActiveInput()
    {
        inputsActive--;
        if (inputsActive == inputsRequired - 1)
            ToggleDoor();
    }

    void ToggleDoor()
    {
        if (doorState == States.closed)
        {
            doorState = States.opening;
            StartCoroutine(OpenDoor());
        }
        else if (doorState == States.open)
        {
            doorState = States.closing;
            StartCoroutine(CloseDoor());
        }
        else if (doorState == States.opening)
        {
            closeDoorWhenAvailable = true;
        }
    }

    public float GetOpenAngle()
    {
        return openAngle + (openFromLeft ? -startAngle : startAngle);
    }

    IEnumerator OpenDoor()
    {
        doorState = States.opening;
        startPos = transform.position;
        float desiredAngle = openFromLeft ? -GetOpenAngle() : GetOpenAngle();
        float angle = 0f;
        float degRotated = 0f;
        float timeKeep = 0f;
        while (timeKeep <= openTime)
        {
            timeKeep += Time.deltaTime;
            angle = desiredAngle * Time.deltaTime / openTime;
            degRotated += angle;
            transform.RotateAround(HingeLocation(), Vector3.up, angle);
            yield return null;
        }
        transform.RotateAround(HingeLocation(), Vector3.up, desiredAngle - degRotated);
        doorState = States.open;
        if (closeDoorWhenAvailable)
        {
            closeDoorWhenAvailable = false;
            StartCoroutine(CloseDoor());
        }
    }

    IEnumerator CloseDoor()
    {
        doorState = States.closing;
        float desiredAngle = openFromLeft ? GetOpenAngle() : -GetOpenAngle();
        float angle = 0f;
        float degRotated = 0f;
        float timeKeep = 0f;
        while (timeKeep <= closeTime)
        {
            timeKeep += Time.deltaTime;
            angle = desiredAngle * Time.deltaTime / closeTime;
            degRotated += angle;
            transform.RotateAround(HingeLocation(), Vector3.up, angle);
            yield return null;
        }
        transform.RotateAround(HingeLocation(), Vector3.up, desiredAngle - degRotated);
        doorState = States.closed;
    }

    Vector3 HingeLocation()
    {
        Vector3 hinge = new Vector3(Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).y),
            boxColl.size.y / 2f + transform.position.y, -Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).y));
        hinge.x *= boxColl.size.x * (openFromLeft ? 0 : 1); hinge.z *= boxColl.size.x * (openFromLeft ? 0 : 1);
        hinge.x += transform.position.x; hinge.z += transform.position.z;
        return hinge;
    }

    private void OnMouseDown()
    {
        if (haxMode)
            ToggleDoor();
    }

    private void OnDrawGizmosSelected()
    {
        if (boxColl != null)
        {
            Vector3 pos = new Vector3(transform.position.x + boxColl.center.x + (openFromLeft ? -boxColl.size.x / 2f : boxColl.size.x / 2f), transform.position.y + boxColl.size.y, transform.position.z);
            Vector3 endPos = new Vector3(Mathf.Cos(Quaternion.ToEulerAngles(transform.rotation).y), boxColl.size.y / 2f + transform.position.y, -Mathf.Sin(Quaternion.ToEulerAngles(transform.rotation).y));
            endPos.x *= boxColl.size.x * (openFromLeft ? 0 : 1); endPos.z *= boxColl.size.x * (openFromLeft ? 0 : 1);
            endPos.x += transform.position.x; endPos.z += transform.position.z;
            Gizmos.DrawWireSphere(endPos, 0.5f);
        }
    }
}
