using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearDispenser : MonoBehaviour
{
    public GameObject spearObj;
    public float refillTime = 1.5f;
    public int capacity = 3;
    int spearsLeft;

    Transform spearSpawn;
    List<Spear> spears = new List<Spear>();

    private void Start()
    {
        spearsLeft = capacity - 1;
        spearSpawn = transform.GetChild(0);
        SpawnSpear();
        PlayerSpear.PickedUp += SpearTaken;
    }

    void SpawnSpear()
    {
        GameObject spawnedSpear = Instantiate(spearObj, spearSpawn.position, Quaternion.identity);
        spawnedSpear.transform.parent = spearSpawn;
        spawnedSpear.transform.rotation = spearSpawn.rotation;
        spawnedSpear.transform.Translate(transform.up * spawnedSpear.GetComponent<BoxCollider>().bounds.extents.x / 2f);
        spears.Add(spawnedSpear.GetComponent<Spear>());
    }

    void SpearTaken(Spear spear)
    {
        if (spears.Contains(spear) && spearsLeft > 0)
        {
            spearsLeft--;
            spears.Remove(spear);
            StartCoroutine(Refill());
        }
    }

    IEnumerator Refill()
    {
        yield return new WaitForSeconds(refillTime);
        SpawnSpear();
    }
}
