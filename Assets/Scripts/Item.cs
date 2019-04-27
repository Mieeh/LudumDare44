using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType{
        WEAPON,
        ARMOR,
        JUNK
    }

    public string itemName;
    [TextArea]
    public string flavourText;
    public ItemType itemType;
    public int value = 0;
    public int damage = 0;
    public int defense = 0;

    private SpriteRenderer spre;

    private void Awake() {
        spre = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        StartCoroutine(ColliderCoroutine());
    }

    private IEnumerator ColliderCoroutine(){
        yield return new WaitForSeconds(1.0f);
        GetComponent<Collider2D>().enabled = true;
    }
}
