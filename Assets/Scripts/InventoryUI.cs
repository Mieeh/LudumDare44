using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Image[] imageSlots;
    public GameObject inventoryGameObject;
    private PlayerInventory playerInventory;
    private PlayerMove playerMove;
    private Item selectedItem;
    public TMP_Text selectedItemName;
    public TMP_Text selectedItemFlavourText;
    public Image selectedItemImage; 

    [Header("Player Window")]
    public TMP_Text playerWeaponName;
    public Image playerWeaponImage;
    public TMP_Text playerArmorName;
    public Image playerArmorImage;
    public TMP_Text playerWeaponDamageText, playerWeaponValueText;
    public TMP_Text playerArmorStatText, playerArmorValueText;

    int desiredIndex = 0;

    private void Start() {
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerMove = FindObjectOfType<PlayerMove>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.I)){
            if(!inventoryGameObject.activeInHierarchy)
                OpenInventory();
            else
                CloseInventory();
        }

        if(inventoryGameObject.activeInHierarchy){
            // Freeze time!
            Time.timeScale = 0.0f;
        }

        if(inventoryGameObject.activeInHierarchy && playerInventory.itemList.Count > 0){
            // Navigating the inventory
            if(Input.GetKeyDown(KeyCode.D)){
                if(desiredIndex < playerInventory.itemList.Count-1)
                    desiredIndex++;
                else
                    desiredIndex = 0;
                
                SelectInventory(desiredIndex);
            }
            else if(Input.GetKeyDown(KeyCode.A)){
                if(desiredIndex != 0)
                    desiredIndex--;
                else
                    desiredIndex = playerInventory.itemList.Count-1;
            
                SelectInventory(desiredIndex);
            }
            else if(Input.GetKeyDown(KeyCode.W)){
                int newIndex = desiredIndex-4;
                if(newIndex >= 0)
                    desiredIndex = newIndex;
                SelectInventory(desiredIndex);
            }
            else if(Input.GetKeyDown(KeyCode.S)){
                int newIndex = desiredIndex+4;
                if(newIndex < playerInventory.itemList.Count-1)
                    desiredIndex = newIndex;
                SelectInventory(desiredIndex);
            }

            if(Input.GetKeyDown(KeyCode.Space)){
                ClickedEquip();
            }
        }
    }

    public void OpenInventory() {
        inventoryGameObject.SetActive(true);

        UpdateInventorySlots();
        UpdatePlayerWindow();
    }

    private void UpdatePlayerWindow(){
        // Weapon
        if(playerInventory.currentWeapon != null){
            playerWeaponName.text = "Weapon: <color=green>" + playerInventory.currentWeapon.itemName + "</color>";
            playerWeaponImage.sprite = playerInventory.currentWeapon.GetComponent<SpriteRenderer>().sprite;
            playerWeaponImage.color = Color.white;

            // Stat texts
            playerWeaponDamageText.text = playerInventory.currentWeapon.damage.ToString();
            playerWeaponValueText.text = playerInventory.currentWeapon.value.ToString();
        }
        else{
            playerWeaponName.text = "No Weapon!";
            playerWeaponImage.color = Color.clear;

            playerWeaponDamageText.text = "";
            playerWeaponValueText.text = "";
        }

        // Armor
        if(playerInventory.currentArmor != null){
            playerArmorName.text = "Armor <color=green>" + playerInventory.currentArmor.itemName + "</color>";
            // Image
            playerArmorImage.sprite = playerInventory.currentArmor.GetComponent<SpriteRenderer>().sprite;
            playerArmorImage.color = Color.white;

            // Stat texts
            playerArmorStatText.text = playerInventory.currentArmor.defense.ToString();
            playerArmorValueText.text = playerInventory.currentArmor.value.ToString();
        }
        else{
            playerArmorName.text = "No Armor!";
            playerArmorImage.color = Color.clear;

            playerArmorStatText.text = "";
            playerArmorValueText.text = "";
        }
    }

    private void UpdateInventorySlots(){
        // Loop through and display items correctly
        for(int i = 0; i < PlayerInventory.ITEM_MAX_COUNT; i++){
            if(i < playerInventory.itemList.Count){
                imageSlots[i].color = Color.white;
                imageSlots[i].sprite = playerInventory.itemList[i].GetComponent<SpriteRenderer>().sprite;
                imageSlots[i].raycastTarget = true;
            }
            else{
                imageSlots[i].color = Color.clear;
                imageSlots[i].raycastTarget = false;
            }
        }

        if(playerInventory.itemList.Count == 0)
            desiredIndex = -1;
        else
            desiredIndex = 0;
        
        SelectInventory(desiredIndex);
    }

    public void CloseInventory(){
        Time.timeScale = 1;
        inventoryGameObject.SetActive(false);
        selectedItem = null;
    }

    public void SelectInventory(int index){
        if(index != -1){
            for(int i = 0; i < playerInventory.itemList.Count; i++){
                if(i != desiredIndex){
                    imageSlots[i].color = new Color(1,1,1,0.3f);
                }
                else{
                    imageSlots[i].color = Color.white;
                }
            }

            selectedItem = playerInventory.itemList[index];
            selectedItemImage.color = Color.white;
            selectedItemImage.sprite = selectedItem.GetComponent<SpriteRenderer>().sprite;
            selectedItemName.text = selectedItem.itemName;
            selectedItemFlavourText.text = selectedItem.flavourText;
        }
        else{
            selectedItem = null;
            selectedItemImage.color = Color.clear;
            selectedItemName.text = "";
            selectedItemFlavourText.text = "";
        }
    }

    public void ClickedEquip() {
        if(selectedItem != null){
            if(selectedItem.itemType != Item.ItemType.JUNK){
                // Update player inventory
                playerInventory.RemoveItem(selectedItem);
                playerInventory.Equip(selectedItem);
                
                // update UI
                UpdateInventorySlots();
                SelectInventory(desiredIndex);
                UpdatePlayerWindow();
            }
        }
    }
}
