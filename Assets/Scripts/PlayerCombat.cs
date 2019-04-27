using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int HP;
    public int attack;
    public BoxCollider2D attackCollider;
    private Rigidbody2D rb;

    private bool canAttack = true;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            Attack();
        }
    }

    private void Attack(){
        
    }

}
