using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCrate : MonoBehaviour
{
    public Item itemInside;

    public GameObject rubblePrefab;
    public GameObject rubbleParticles;

    public void GetDestroyed(){
        // Get destroyed
        // Spawn item + rubble + particles and then destroy this gameobject
        Instantiate(itemInside.gameObject, transform.position, Quaternion.identity);
        Instantiate(rubblePrefab, transform.position, Quaternion.identity);
        GameObject _particles = Instantiate(rubbleParticles, transform.position, Quaternion.identity) as GameObject;
        Destroy(_particles, 3.0f); // Some amount of time

        Destroy(gameObject); // Done
    }
}
