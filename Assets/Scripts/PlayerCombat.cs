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
    [System.NonSerialized]
    public int HP = 100;
    [System.NonSerialized]
    public int attack = 10;

    [Header("Components & Other")]
    public BoxCollider2D attackCollider;
    private Rigidbody2D rb;
    private PlayerMove playerMove;
    private PlayerInventory playerInventory;
    private List<Vector3> attackColliderPositions = new List<Vector3>();
    public float stunTimer = 1.0f;

    private AttackDirection attackDirection;
    private bool isAttacking = false;
    public float attackLengthInSeconds;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMove>();
        playerInventory = GetComponent<PlayerInventory>();

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
        if(isAttacking || playerMove.isDodging)
            return;

        StartCoroutine("AttackCoroutine");
    }

    public void GetAttacked(EnemyBase enemy){
        int damage = ConvertToPlayerDamage(enemy.attack);
        HP-=damage;
        if(HP <= 0){
            print("DIEDIEDIEDIEDIEDIE");
        }

        print("Player Attacked!");

        StartCoroutine(AttackedCoroutine(enemy.transform.position));
    }

    public void GetAttacked(Vector2 position, int howMuch){
        int damage = ConvertToPlayerDamage(howMuch);
    }

    public int GetPlayerAttack(){
        int _attack = attack;
        if(playerInventory.currentWeapon != null)
            _attack += playerInventory.currentWeapon.damage;

        return _attack;
    }

    public int ConvertToPlayerDamage(int attack){
        if(playerInventory.currentArmor != null){
            return attack * (playerInventory.currentArmor.defense/100);
        }
        return attack;
    }

    public int GetPlayerKnockBack(){
        
        if(playerInventory.currentWeapon != null){
            return playerInventory.currentWeapon.knockBack;
        }

        return 0;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Projectile"){
            Destroy(other.gameObject);
            GetAttacked()
        }
    }

    private IEnumerator AttackedCoroutine(Vector2 position){
        isAttacking = true;
        playerMove.enabled = false;
        rb.velocity = Vector2.zero;
        
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        GetComponent<Collider2D>().enabled = false;

        // Apply the knockback force
        Vector2 dir = (Vector3)position - transform.position;
        dir*=-1;
        dir.Normalize();
        rb.AddForce(dir*200);

        yield return new WaitForSeconds(stunTimer*0.3f);

        playerMove.enabled = true;
        isAttacking = false;

        yield return new WaitForSeconds(stunTimer*0.7f);

        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<Collider2D>().enabled = true;
    }

    private IEnumerator AttackCoroutine(){
        isAttacking = true;
        // Enable the collider
        attackCollider.enabled = true;
        // Position the collider accordingly
        attackCollider.transform.localPosition = attackColliderPositions[(int)attackDirection];

        // Disable movement
        playerMove.enabled = false;
        rb.velocity = Vector2.zero;
        
        yield return new WaitForSeconds(attackLengthInSeconds);
        
        // Enable movement
        playerMove.enabled = true;

        // Disable the collider
        attackCollider.enabled = false;
        attackCollider.transform.localPosition = Vector2.zero;
        isAttacking = false;
    }

}
