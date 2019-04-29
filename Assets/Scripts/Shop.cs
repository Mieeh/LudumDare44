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
    public Image wantedItemImage, wantedItemImage2;

    private PlayerInventory playerInventory;
    private PlayerCombat playerCombat;
    private int desiredIndex = 0;

    public Buyer[] allPossibleBuyers;
    private Buyer currentBuyer;

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

                // SFX
                SoundEffectsSystem.PlaySFX("ui_scroll_up");

                UpdateCurrentItem();
            }
            if(Input.GetKeyDown(KeyCode.S)){
                if(desiredIndex < playerInventory.itemList.Count-1){
                    desiredIndex++;
                }
                else{
                    desiredIndex = 0;
                }

                // SFX
                SoundEffectsSystem.PlaySFX("ui_scroll_down");

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

        for(int i = 0; i < allPossibleBuyers.Length; i++){
            Color c = allPossibleBuyers[i].GetComponent<SpriteRenderer>().color;

            if(currentBuyer == allPossibleBuyers[i]){
                currentBuyer.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else{
                allPossibleBuyers[i].GetComponent<SpriteRenderer>().color = Color.clear;
            }
        }
    }

    public void StartShop(){

        // Select random buyer
        SelectRandomBuyer();

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
        if(currentBuyer.IsHappyWithItem(itemToSell.itemName)){
            int extraPay = (int)((itemToSell.value * Buyer.payFactor) - itemToSell.value);
            playerCombat.HP += itemToSell.value + extraPay;

            //SFX
            SoundEffectsSystem.PlaySFX("sold_expensive_item");
        }
        else{
            playerCombat.HP += itemToSell.value;

            // SFX
            SoundEffectsSystem.PlaySFX("sold_item");
        }

        // Remove the item!
        playerInventory.itemList.Remove(itemToSell);
        Destroy(itemToSell.gameObject);

        desiredIndex = 0;

        // New buyer
        SelectRandomBuyer();

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

        // Is this something that the buyer wants?
        bool buyerWants = currentBuyer.IsHappyWithItem(_item.itemName);

        int extraPay = (int)((_item.value * Buyer.payFactor) - _item.value);

        if(buyerWants){
            itemValue.text = "<color=red>" + _item.value.ToString() + "</color> + <color=green>" +  extraPay + "</color>";
        }
        else{
            itemValue.text = "<color=red>" + _item.value.ToString();
        }
        itemDamage.text = _item.damage.ToString();
        itemDefense.text = _item.defense.ToString();
        
    }

    private void SelectRandomBuyer(){
        int randIndex = Random.Range(0, allPossibleBuyers.Length);
        currentBuyer = allPossibleBuyers[randIndex];

        // Update UI
        wantedItemImage.sprite = currentBuyer.wantedItem.GetComponent<SpriteRenderer>().sprite;
        wantedItemImage2.sprite = currentBuyer.wantedItem2.GetComponent<SpriteRenderer>().sprite;
    }

}
