using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static int ITEM_MAX_COUNT = 16;

    [System.NonSerialized]
    public List<Item> itemList = new List<Item>();
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
        if(itemList.Count < ITEM_MAX_COUNT){
            itemToAdd.gameObject.SetActive(false);
            itemList.Add(itemToAdd);
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
            if(currentWeapon != null){
                AddItem(currentWeapon);
            }
            currentWeapon = itemToEquip;
        }
        else{
            if(currentArmor != null){
                AddItem(currentArmor);
            }
            currentArmor = itemToEquip;
        }
    }

    public void RemoveItem(Item itemToRemove){
        if(itemList.Contains(itemToRemove)){
            itemList.Remove(itemToRemove);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Item>() != null){
            AddItem(other.GetComponent<Item>());
        }
    }
}
