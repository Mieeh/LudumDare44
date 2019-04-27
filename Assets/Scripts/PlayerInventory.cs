using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    static int ITEM_MAX_COUNT = 16;

    [System.NonSerialized]
    public int itemCount = 0;
    public Item[] itemList = new Item[ITEM_MAX_COUNT];

    public Item currentWeapon;
    public Item currentArmor;

    private void Update() {
        if(currentArmor != null){
            currentArmor.gameObject.SetActive(false);
        }
        if(currentWeapon != null){
            currentWeapon.gameObject.SetActive(false);
        }
    }
    
    public void AddItem(Item itemToAdd){

        // @ Maybe auto equip if we dont have any weapon or armor equiped
        if(itemCount < ITEM_MAX_COUNT){
            itemToAdd.gameObject.SetActive(false);
            itemList[itemCount] = itemToAdd;
            itemCount++;
        }
        else{
            print("Max capacity reached!");
        }
    }

    public void Equip(Item itemToEquip){
        if(itemToEquip.itemType == Item.ItemType.JUNK){
            print("You can't equip junk!");
            return;
        }

        if(itemToEquip.itemType == Item.ItemType.WEAPON){
            currentWeapon = itemToEquip;
        }
        else{
            currentArmor = itemToEquip;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Item"){
            AddItem(other.GetComponent<Item>());
        }
    }
}
