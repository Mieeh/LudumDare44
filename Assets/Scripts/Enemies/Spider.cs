using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : EnemyBase
{
    // Inherits:
    // HP: int, attack: int

    private Vector2 goalPos;
    public float patrollingSpeed = 1.0f;
    public float chaseSpeed = 2.0f;

    private void Start() {
        BaseStart();

        goalPos = GetRandomPointInsideZone();
    }

    private void Update() {
        if(enemyState == EnemyState.PATROLLING && !knockedBack){
            // Move towards goalPos
            float distance = ((Vector2)transform.position - goalPos).magnitude; 
            if(distance > 0.1f) {
                MoveRigidbodyTowards(goalPos, patrollingSpeed);
            }
            else{
                goalPos = GetRandomPointInsideZone();
            }
        }
        else if(enemyState == EnemyState.CHASING && !knockedBack){
            // Chase the player!
            float distance = (playerTransform.position - transform.position).magnitude;
            if(distance > 0.1f){
                MoveRigidbodyTowards((Vector2)playerTransform.position, chaseSpeed);
            }
        }
    }

    public override void TakeDamage(int howMuch, float knockBack){
        HP-=howMuch;
    
        // Knockback away from the player
        StopCoroutine("GetKnockedBack");
        StartCoroutine(GetKnockedBack(knockBack));
        
        // Did we die?
        if(HP <= 0)
            Die();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Is the player within our sight? 
        // If he is, start chasing him!
        if(other.GetComponent<PlayerCombat>() != null){
            enemyState = EnemyState.CHASING;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        // Stop chasing the player?
        if(other.GetComponent<PlayerCombat>() != null){
            enemyState = EnemyState.PATROLLING;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.GetComponent<PlayerMove>() != null)
        {
            other.gameObject.GetComponent<PlayerCombat>().GetAttacked(this);
        }
    }

}
