using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buyer : MonoBehaviour
{
    public Item wantedItem, wantedItem2;
    public static float payFactor = 1.1f;

    public bool IsHappyWithItem(string itemName){
        if(itemName == wantedItem.itemName || itemName == wantedItem2.itemName)
            return true;

        return false;
    }

}
