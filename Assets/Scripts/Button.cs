using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Button : MonoBehaviour
{
    enum States { released, pressing, pressed, releasing }
    enum Requests { press, release, nothing }

    States buttonState = new States();
    Requests buttonRequest = new Requests(); // I can use a bool but this one has pretty colors

    public int requiredWeight = 1;
    [Range(0.1f, 0.49f)]
    public float pushMagnitude = 0.25f;
    public float pressTime = 0.1f;
    public float releaseTime = 0.2f;
    public Door[] linkedDoors;
    public bool haxMode;
    [Space]
    public GameObject wireHolder;
    public Material wireOn;
    public Material wireOff;

    MeshRenderer[] wire;
    HashSet<Transform> itemsInContact = new HashSet<Transform>();

    private void Start()
    {
        buttonState = States.released;
        buttonRequest = Requests.nothing;
        if (wireHolder != null)
        {
            wire = wireHolder.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < wire.Length; i++)
            {
                wire[i].material = wireOff;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Spear spear = other.GetComponent<Spear>();
        if (spear != null)
        {
            if (!itemsInContact.Contains(other.transform))
            {
                itemsInContact.Add(other.transform);
                if (itemsInContact.Count == requiredWeight)
                {
                    if (buttonState == States.released && buttonRequest != Requests.release)
                        StartCoroutine(AnimatePress(transform.position));
                    else if (buttonState == States.releasing)
                        buttonRequest = Requests.press;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Spear spear = other.GetComponent<Spear>();
        if (spear != null)
        {
            if (itemsInContact.Contains(other.transform))
            {
                itemsInContact.Remove(other.transform);
                if (itemsInContact.Count == requiredWeight - 1)
                {
                    if (buttonState == States.pressed && buttonRequest != Requests.press)
                        StartCoroutine(AnimateRelease(transform.position));
                    else if (buttonState == States.pressing)
                        buttonRequest = Requests.release;
                    else if (buttonState == States.releasing)
                        buttonRequest = Requests.nothing;
                }
            }
        }
    }

    //This is necessary and i kid you not, it actually fucked up ONCE without this
    private void OnTriggerStay(Collider other)
    {
        if (itemsInContact.Count == 0)
        {
            Spear spear = other.GetComponent<Spear>();
            if (spear != null)
            {
                if (!itemsInContact.Contains(other.transform))
                {
                    itemsInContact.Add(other.transform);
                    if (itemsInContact.Count == requiredWeight)
                    {
                        if (buttonState == States.released && buttonRequest != Requests.release)
                            StartCoroutine(AnimatePress(transform.position));
                        else if (buttonState == States.releasing)
                            buttonRequest = Requests.press;
                    }
                }
            }
        }
    }

    protected virtual void Activate()
    {
        if (wire != null)
        {
            for (int i = 0; i < wire.Length; i++)
            {
                wire[i].material = wireOn;
            }
        }

        for (int i = 0; i < linkedDoors.Length; i++)
        {
            linkedDoors[i].AddActiveInput(this);
        }
    }

    void Deactivate()
    {
        if (wire != null)
        {
            for (int i = 0; i < wire.Length; i++)
            {
                wire[i].material = wireOff;
            }
        }

        for (int i = 0; i < linkedDoors.Length; i++)
        {
            linkedDoors[i].SubtractActiveInput(this);
        }
    }

    IEnumerator AnimatePress(Vector3 startPos)
    {
        buttonState = States.pressing;
        Vector3 endPos = -transform.right * -pushMagnitude + startPos;
        float xScale = transform.localScale.x;
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime / pressTime;
            progress = Mathf.Clamp(progress, 0f, 1f);
            transform.position = Vector3.Lerp(startPos, endPos, progress);
            transform.localScale = new Vector3(xScale - pushMagnitude * progress, transform.localScale.y, transform.localScale.z);
            yield return null;
        }
        buttonState = States.pressed;
        Activate();
        if (buttonRequest == Requests.release)
        {
            buttonRequest = Requests.nothing;
            StartCoroutine(AnimateRelease(transform.position));
        }
    }

    IEnumerator AnimateRelease(Vector3 startPos)
    {
        buttonState = States.releasing;
        Vector3 endPos = transform.right * -pushMagnitude + startPos;
        float xScale = transform.localScale.x;
        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime / releaseTime;
            progress = Mathf.Clamp(progress, 0f, 1f);
            transform.localScale = new Vector3(xScale + pushMagnitude * progress, transform.localScale.y, transform.localScale.z);
            transform.position = Vector3.Lerp(startPos, endPos, progress);
            yield return null;
        }
        buttonState = States.released;
        Deactivate();
        if (buttonRequest == Requests.press)
        {
            buttonRequest = Requests.nothing;
            StartCoroutine(AnimatePress(transform.position));
        }
    }

    #region Debug

    private void OnMouseDown()
    {
        if (haxMode)
        {
            if (buttonState == States.released && buttonRequest != Requests.release)
                StartCoroutine(AnimatePress(transform.position));
            else if (buttonState == States.releasing)
                buttonRequest = Requests.press;
        }
    }

    private void OnMouseUp()
    {
        if (haxMode)
        {
            if (buttonState == States.pressed && buttonRequest != Requests.press)
                StartCoroutine(AnimateRelease(transform.position));
            else if (buttonState == States.pressing)
                buttonRequest = Requests.release;
            else if (buttonState == States.releasing)
                buttonRequest = Requests.nothing;
        }
    }

    #endregion
}
