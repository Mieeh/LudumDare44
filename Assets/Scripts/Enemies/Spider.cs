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
    public float chaseRange = 4.0f;

    private void Start() {
        BaseStart();

        goalPos = GetRandomPointInsideZone();
    }

    private void Update() {

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if(distanceToPlayer < chaseRange){
            enemyState = EnemyState.CHASING;
        }
        else{
            enemyState = EnemyState.PATROLLING;
        }

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

        // Animation directionality
        Vector2 velocityVector = rBody.velocity.normalized;
        animator.SetFloat("walk_x", velocityVector.x);
        animator.SetFloat("walk_y", velocityVector.y);
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

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.GetComponent<PlayerMove>() != null)
        {
            other.gameObject.GetComponent<PlayerCombat>().GetAttacked(this);
            animator.SetTrigger("attack_trigger");
        }
    }

}
