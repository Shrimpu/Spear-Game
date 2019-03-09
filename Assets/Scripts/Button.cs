using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Button : MonoBehaviour
{
    public int requiredWeight = 1;
    public Door[] linkedDoors;

    HashSet<Transform> itemsInContact = new HashSet<Transform>();

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
                    for (int i = 0; i < linkedDoors.Length; i++)
                    {
                        linkedDoors[i].ToggleDoor();
                    }
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
                if (itemsInContact.Count == 0)
                {
                    for (int i = 0; i < linkedDoors.Length; i++)
                    {
                        linkedDoors[i].ToggleDoor();
                    }
                }
            }
        }
    }

    #region Debug

    private void OnMouseDown()
    {
        for (int i = 0; i < linkedDoors.Length; i++)
        {
            linkedDoors[i].ToggleDoor();
        }
    }

    private void OnMouseUp()
    {
        for (int i = 0; i < linkedDoors.Length; i++)
        {
            linkedDoors[i].ToggleDoor();
        }
    }

    #endregion
}
