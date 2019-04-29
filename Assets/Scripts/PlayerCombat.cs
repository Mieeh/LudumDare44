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
    public int HP = 100;
    public int attack = 10;

    [Header("Components & Other")]
    public BoxCollider2D attackCollider;
    private Rigidbody2D rb;
    private PlayerMove playerMove;
    private PlayerInventory playerInventory;
    private SpriteRenderer spre;
    private Animator animController;
    private List<Vector3> attackColliderPositions = new List<Vector3>();
    public float stunTimer = 1.0f;
    
    public Sprite[] idleSprites;

    private AttackDirection attackDirection;
    private bool isAttacking = false;
    public float attackLengthInSeconds;
    [System.NonSerialized] 
    public bool attackInvincibility = false;
    private CameraFollow cam;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMove>();
        playerInventory = GetComponent<PlayerInventory>();
        spre = GetComponent<SpriteRenderer>();
        animController = GetComponent<Animator>();
        cam = FindObjectOfType<CameraFollow>(); // cam shake

        attackColliderPositions.Add(new Vector3(0, 0.56f));
        attackColliderPositions.Add(new Vector3(0.75f, -0.33f));
        attackColliderPositions.Add(new Vector3(0, -0.95f));
        attackColliderPositions.Add(new Vector3(-0.75f, -0.33f));
    }

    void Update(){
        // Input for attack
        if(Input.GetKeyDown(InputKeys.ATTACK_KEY)){
            Attack();
        }

        // Check attack direction
        if(rb.velocity.x > 0){
            attackDirection = AttackDirection.RIGHT;
            animController.SetFloat("attack_direction", 0);
        }
        else if(rb.velocity.x < 0){
            attackDirection = AttackDirection.LEFT;
            animController.SetFloat("attack_direction", 1);
        }
        else if(rb.velocity.y > 0){
            attackDirection = AttackDirection.UP;
            animController.SetFloat("attack_direction", 2);
        }
        else if(rb.velocity.y < 0){
            attackDirection = AttackDirection.DOWN;
            animController.SetFloat("attack_direction", 3);
        }

        // Walk direction
        Vector2 velocityVector = rb.velocity.normalized;
        animController.SetFloat("x", velocityVector.x);
        animController.SetFloat("y", velocityVector.y);

        if(velocityVector.x == 0 && velocityVector.y == 0 && !isAttacking){
            animController.enabled = false;
            spre.sprite = idleSprites[(int)attackDirection];
        }
        else{
            animController.enabled = true;
            
        }
    }

    private void Attack(){
        // If we're already attacking, STOP and return 
        if(isAttacking || playerMove.isDodging || attackInvincibility)
            return;

        animController.SetTrigger("attack_trigger");

        // SFX
        SoundEffectsSystem.PlaySFX("player_attack");

        StopCoroutine("AttackCoroutine");
        StartCoroutine("AttackCoroutine");
    }

    public void GetAttacked(EnemyBase enemy){
        if(playerMove.isInvincible || attackInvincibility){
            return;
        }

         // Spawn blooderino
        FindObjectOfType<GameMaster>().SpawnSlashBlood(transform.position);

        // Other effects
        cam.LightShake();
        SoundEffectsSystem.PlaySFX("player_damaged");

        int damage = ConvertToPlayerDamage(enemy.attack);
        HP-=damage;
        if(HP <= 0){
            FindObjectOfType<GameMaster>().GameOver();
        }

        StartCoroutine(AttackedCoroutine(enemy.transform.position));
    }

    public void GetAttacked(Vector2 position, int howMuch){
        if(playerMove.isInvincible){
            return;
        }

        // Spawn blooderino
        FindObjectOfType<GameMaster>().SpawnSlashBlood(transform.position);

        // Other effects
        cam.LightShake();
        SoundEffectsSystem.PlaySFX("player_damaged");

        int damage = ConvertToPlayerDamage(howMuch);
        HP-=damage;
        if(HP <= 0){
            FindObjectOfType<GameMaster>().GameOver();
        }

        StartCoroutine(AttackedCoroutine(position));
    }

    public int GetPlayerAttack(){
        int _attack = attack; // Base attack
        if(playerInventory.currentWeapon != null)
            _attack = playerInventory.currentWeapon.damage;

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
            if(!playerMove.isInvincible || !attackInvincibility){
                GetAttacked(other.transform.position, other.GetComponent<Projectile>().damage);
                Destroy(other.gameObject);
            }
        }
        if(other.tag == "Entrance"){
            // Goto SHOP!
            FindObjectOfType<GameMaster>().GotoShop();
        }
        if(other.tag == "OgreAttackCollider"){
            if(!playerMove.isInvincible || !attackInvincibility){
                GetAttacked(other.GetComponentInParent<EnemyBase>());
            }
        }
    }

    private IEnumerator AttackedCoroutine(Vector2 position){
        isAttacking = true;
        playerMove.enabled = false;
        rb.velocity = Vector2.zero;
        
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        attackInvincibility = true;

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
        attackInvincibility = false;
    }

    private IEnumerator AttackCoroutine(){
        isAttacking = true;

        // Disable movement
        playerMove.enabled = false;
        rb.velocity = Vector2.zero;

        // Enable the collider
        attackCollider.enabled = true;
        // Position the collider accordingly
        attackCollider.transform.localPosition = attackColliderPositions[(int)attackDirection];
        if(attackDirection == AttackDirection.RIGHT || attackDirection == AttackDirection.LEFT){
            attackCollider.transform.localScale = new Vector3(0.6f,  1.15f, 1.0f);
        }
        else if(attackDirection == AttackDirection.UP){
            attackCollider.transform.localScale = new Vector3(1.4f, 0.65f, 1.0f);
        }
        else if(attackDirection == AttackDirection.DOWN){
            attackCollider.transform.localScale = new Vector3(1.4f, 0.545f);
        }
        
        yield return new WaitForSeconds(attackLengthInSeconds);
        
        // Enable movement
        playerMove.enabled = true;

        // Disable the collider
        attackCollider.enabled = false;
        attackCollider.transform.localPosition = Vector2.zero;
        isAttacking = false;
    }

}
