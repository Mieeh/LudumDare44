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
    public List<Item> itemPool;
    [Range(0, 1)]
    public float chanceOfDropping;
    protected EnemyState enemyState = EnemyState.PATROLLING;


    protected bool knockedBack = false;
    protected Rigidbody2D rBody;
    protected PlayerCombat playerCombat;
    protected PlayerMove playerMove;
    protected Transform playerTransform;
    public BoxCollider2D patrollingZone;

    protected void BaseStart(){
        rBody = GetComponent<Rigidbody2D>();
        playerCombat = FindObjectOfType<PlayerCombat>();
        playerMove = FindObjectOfType<PlayerMove>();
        playerTransform = playerMove.transform;
    }

    public abstract void TakeDamage(int howMuch, float knockBack);

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

        // Spawn item?
        if(itemPool.Count > 0) {
            float rand = Random.Range(0.0f, 1.0f);
            if(rand >= 1.0f-chanceOfDropping){
                // Yes, we drop some random item from our pool
                int randIndex = Random.Range(0, itemPool.Count);
                GameObject spawnedItem = Instantiate(itemPool[randIndex].gameObject, transform.position, Quaternion.identity) as GameObject;
            }
        }

        Destroy(gameObject);
    }
}
