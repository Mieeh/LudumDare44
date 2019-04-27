using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBehaviour : MonoBehaviour
{
    public static int MAX_ITEMS = 16;
    public PlayerInventory inventory;
    public List<GameObject> itemSlots = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<PlayerInventory>();
        UpdateSellableItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            UpdateSellableItems();
        }
    }

    void UpdateSellableItems() {
        for (int i = 0; i < MAX_ITEMS; i++) {
            print(inventory.itemList.Count);
            if (i < inventory.itemList.Count) {
                itemSlots[i].GetComponent<SpriteRenderer>().sprite = inventory.itemList[i].GetComponent<SpriteRenderer>().sprite;
                itemSlots[i].GetComponent<SpriteRenderer>().color = Color.white;
            } else {
                itemSlots[i].GetComponent<SpriteRenderer>().color = Color.clear;
            }
        }
    }
}
