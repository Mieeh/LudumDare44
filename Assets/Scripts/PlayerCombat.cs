using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public enum AttackDirection{
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3,
        ERROR = -1
    };

    [Header("Player Stats")]
    public int HP;
    public int attack;

    [Header("Components & Other")]
    public BoxCollider2D attackCollider;
    private Rigidbody2D rb;
    private List<Vector3> attackColliderPositions = new List<Vector3>();

    private AttackDirection attackDirection;
    private bool isAttacking = false;
    public float attackLengthInSeconds;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        attackColliderPositions.Add(new Vector3(0, 1));
        attackColliderPositions.Add(new Vector3(1, 0));
        attackColliderPositions.Add(new Vector3(0, -1));
        attackColliderPositions.Add(new Vector3(-1, 0));
    }

    void Update(){
        // Input for attack
        if(Input.GetKeyDown(KeyCode.Space)){
            Attack();
        }

        // Check attack direction
        if(rb.velocity.x > 0){
            attackDirection = AttackDirection.RIGHT;
        }
        else if(rb.velocity.x < 0){
            attackDirection = AttackDirection.LEFT;
        }
        else if(rb.velocity.y > 0){
            attackDirection = AttackDirection.UP;
        }
        else if(rb.velocity.y < 0){
            attackDirection = AttackDirection.DOWN;
        }
    }

    private void Attack(){
        // If we're already attacking, STOP and return 
        if(isAttacking)
            return;

        StartCoroutine("AttackCoroutine");
    }

    private IEnumerator AttackCoroutine(){
        isAttacking = true;
        // Enable the collider
        attackCollider.enabled = true;
        // Position the collider accordingly
        attackCollider.transform.localPosition = attackColliderPositions[(int)attackDirection];

        // Disable movement
        GetComponent<PlayerMove>().enabled = false;
        rb.velocity = Vector2.zero;
        
        yield return new WaitForSeconds(attackLengthInSeconds);
        
        // Enable movement
        GetComponent<PlayerMove>().enabled = true;

        // Disable the collider
        attackCollider.enabled = false;
        attackCollider.transform.localPosition = Vector2.zero;
        isAttacking = false;
    }

}
