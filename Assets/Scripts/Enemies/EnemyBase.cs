using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Base Class for all enemies in the game */
public abstract class EnemyBase : MonoBehaviour
{
    public enum EnemyState{
        PATROLLING,
        CHASING,
    }

    public int HP;
    public int attack;
    public float knockStunTime; // For how many seconds is the enemy supposed to be "stunned"
    public List<Item> junkItems = new List<Item>();
    public List<Item> equipmentItems = new List<Item>();
    
    private const float rollChance = 1.0f-0.16f;

    protected EnemyState enemyState = EnemyState.PATROLLING;

    protected bool knockedBack = false;
    protected Rigidbody2D rBody;
    protected PlayerCombat playerCombat;
    protected PlayerMove playerMove;
    protected Transform playerTransform;
    public BoxCollider2D patrollingZone;
    protected Animator animator;

    // Start values used when reseting enemy
    private int startHP;
    private Vector3 startPosition;

    protected void BaseStart(){
        rBody = GetComponent<Rigidbody2D>();
        playerCombat = FindObjectOfType<PlayerCombat>();
        playerMove = FindObjectOfType<PlayerMove>();
        playerTransform = playerMove.transform;
        animator = GetComponent<Animator>();

        // Copy down starting values
        startHP = HP;
        startPosition = transform.position;
    }

    public abstract void TakeDamage(int howMuch, float knockBack);

    public virtual void DungeonReset() { }

    // Returns a random point that's inside the patrolling zone collider bounds
    protected Vector2 GetRandomPointInsideZone(){
        float x = Random.Range(patrollingZone.bounds.min.x, patrollingZone.bounds.max.x);
        float y = Random.Range(patrollingZone.bounds.min.y, patrollingZone.bounds.max.y);
        return new Vector2(x, y);
    }

    protected void MoveRigidbodyTowards(Vector2 target, float speed){
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();

        rBody.velocity = direction * speed;
    }

    protected IEnumerator GetKnockedBack(float knockBack){

        // Enemy damaged SFX
        SoundEffectsSystem.PlaySFX("enemy_damaged");

        knockedBack = true;
        rBody.velocity = Vector2.zero;

        Vector2 dir = playerTransform.position - transform.position;
        dir.Normalize();
        dir *= -1;
        rBody.AddForce(dir*knockBack);

        yield return new WaitForSeconds(knockStunTime);

        knockedBack = false;
    }

    protected void Die(){

        // Spawn blooderino
        FindObjectOfType<GameMaster>().SpawnDeathBlood(transform.position);

        // Spawn item?
        bool dropJunk = false, dropEquipment = false;

        float rand = Random.Range(0.0f, 1.0f);
        // Roll for junk drop
        if(rand >= rollChance){
            dropJunk = true;
            // Roll for equipment
            rand = Random.Range(0.0f, 1.0f);
            if(rand >= rollChance){
                dropEquipment = true;
            }
        }

        if(dropEquipment == true){
            if(equipmentItems.Count != 0){
                // Drop random equipment from the equpmentItems list
                int randIndex = Random.Range(0, equipmentItems.Count);
                GameObject item = Instantiate(equipmentItems[randIndex].gameObject, transform.position, Quaternion.identity) as GameObject;
                item.transform.SetParent(transform.parent);
            }
            else{
                // Drop random junk
                int randIndex = Random.Range(0, junkItems.Count);
                GameObject item = Instantiate(junkItems[randIndex].gameObject, transform.position, Quaternion.identity) as GameObject;
                item.transform.SetParent(transform.parent);
            }
        }
        else if(dropJunk == true){
            // Drop junk
            int randIndex = Random.Range(0, junkItems.Count);
            GameObject item = Instantiate(junkItems[randIndex].gameObject, transform.position, Quaternion.identity) as GameObject;
            item.transform.SetParent(transform.parent);
        } 

        // Disable, basically remove the enemy 
        rBody.simulated = false;
        foreach(var colliders in GetComponents<Collider2D>()){
            colliders.enabled = false;
        }
        GetComponent<SpriteRenderer>().enabled = false;

        // Stop all coroutines lmao
        StopAllCoroutines();
    }

    public void ResetMe(){
        // Reset stats
        HP = startHP;
        transform.position = startPosition;
        enemyState = EnemyState.PATROLLING;
        knockedBack = false;

        // Enable all components again
        rBody.simulated = true;
        foreach(var colliders in GetComponents<Collider2D>()){
            colliders.enabled = true;
        }
        GetComponent<SpriteRenderer>().enabled = true;

        // Inherited thing gets to know we got reset yo
        DungeonReset();
    }
}
