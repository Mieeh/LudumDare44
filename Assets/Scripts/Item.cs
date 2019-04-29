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
    [Header("Weapon")]
    public int damage = 0;
    public int knockBack = 0;
    [Header("Armor")]
    public int defense = 0;

    private SpriteRenderer spre;
    private Collider2D col;
    private Transform playerTransform;

    private void Awake() {
        spre = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        playerTransform = FindObjectOfType<PlayerMove>().transform;
    }

    private void Start() {
        StartCoroutine(ColliderCoroutine());
    }
    
    private void Update() {
        if(col.isActiveAndEnabled){
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if(distanceToPlayer < 2.0f){
                transform.position = Vector3.Lerp(transform.position, playerTransform.position, 1.0f*Time.deltaTime);
            }
        }
    }

    private IEnumerator ColliderCoroutine(){
        yield return new WaitForSeconds(1.0f);
        col.enabled = true;
    }
}
