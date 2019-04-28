using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCrate : MonoBehaviour
{
    public Item itemInside;

    public GameObject rubblePrefab;
    public GameObject rubbleParticles;

    public void GetDestroyed(){
        // Spawn item + rubble + particles and then destroy this gameobject
        GameObject item = Instantiate(itemInside.gameObject, transform.position, Quaternion.identity) as GameObject;
        item.transform.SetParent(transform.parent);

        GameObject rubble = Instantiate(rubblePrefab, transform.position, Quaternion.identity) as GameObject;
        rubble.transform.SetParent(transform.parent);

        GameObject _particles = Instantiate(rubbleParticles, transform.position, Quaternion.identity) as GameObject;
        Destroy(_particles, 3.0f); // Some amount of time

        Destroy(gameObject); // Done
    }
}
