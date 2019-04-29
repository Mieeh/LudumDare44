using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buyer : MonoBehaviour
{
    public Item wantedItem;
    public int extraPay;

    public bool IsHappyWithItem(string itemName){
        if(itemName == wantedItem.itemName)
            return true;

        return false;
    }

}
