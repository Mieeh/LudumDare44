using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    [Header("UI")]
    public GameObject shopHolderObject;
    public TMP_Text itemName;
    public TMP_Text itemFlavourText;
    public Image itemImage;
    public TMP_Text itemValue, itemDamage, itemDefense;
    public TMP_Text playerValueText;

    private PlayerInventory playerInventory;
    private PlayerCombat playerCombat;
    private int desiredIndex = 0;

    private void Awake() {
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerCombat = FindObjectOfType<PlayerCombat>();
    }

    private void Update() {
        if(shopHolderObject.activeInHierarchy && playerInventory.itemList.Count != 0) {
            // Navigating the inventory
            if(Input.GetKeyDown(KeyCode.W)){
                if(desiredIndex == 0){
                    desiredIndex = playerInventory.itemList.Count-1;
                }
                else{
                    desiredIndex--;
                }

                UpdateCurrentItem();
            }
            if(Input.GetKeyDown(KeyCode.S)){
                if(desiredIndex < playerInventory.itemList.Count-1){
                    desiredIndex++;
                }
                else{
                    desiredIndex = 0;
                }

                UpdateCurrentItem();
            }
            if(Input.GetKeyDown(KeyCode.Space)){
                SellCurrentItem();
            }

        }
        playerValueText.text = playerCombat.HP.ToString();

        if(Input.GetKeyDown(KeyCode.X)){
            FindObjectOfType<GameMaster>().GotoDungeon();
        }
    }

    public void StartShop(){
        shopHolderObject.SetActive(true);

        if(playerInventory.itemList.Count != 0){
            desiredIndex = 0;
        }
        else{
            desiredIndex = -1;
        }

        UpdateCurrentItem();
    }

    public void CloseShop(){
        shopHolderObject.SetActive(false);
    }

    public void SellCurrentItem(){
        if(desiredIndex == -1){
            return;
        }

        Item itemToSell = playerInventory.itemList[desiredIndex];

        // Transfer currency to the player
        playerCombat.HP += itemToSell.value;

        // Remove the item!
        playerInventory.itemList.Remove(itemToSell);
        Destroy(itemToSell.gameObject);

        desiredIndex = 0;

        // Update UI
        UpdateCurrentItem();
    }

    public void UpdateCurrentItem(){

        //playerValueText.text = "You have: " + playerCombat.HP.ToString();

        if(playerInventory.itemList.Count == 0){
            itemName.text = "No item left!";

            itemFlavourText.text = "";
            itemImage.color = Color.clear;

            itemValue.text = "0";
            itemDamage.text = "0";
            itemDefense.text = "0";

            desiredIndex = -1;
            return;
        }

        Item _item = playerInventory.itemList[desiredIndex];
        itemName.text = _item.itemName;
        itemFlavourText.text = _item.flavourText;

        itemImage.sprite = _item.GetComponent<SpriteRenderer>().sprite;
        itemImage.color = Color.white;

        itemValue.text = _item.value.ToString();
        itemDamage.text = _item.damage.ToString();
        itemDefense.text = _item.defense.ToString();
        
    }

}
