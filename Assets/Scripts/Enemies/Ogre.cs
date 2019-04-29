using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : EnemyBase
{

    private Vector2[] ogreAttackPositions = new Vector2[4];

    public float patrollingSpeed = 1.0f;
    public float chasingSpeed = 2.0f;

    private Vector2 goalPos;

    private const float chaseRange = 5.0f;
    private const float attackRange = 1.5f;

    private bool isAttacking = false;

    public Collider2D attackCollider;
    private SpriteRenderer spre;

    private void Start() {
        BaseStart();

        StartCoroutine("ChangeDirection");

        goalPos = GetRandomPointInsideZone();

        spre = GetComponent<SpriteRenderer>();

        ogreAttackPositions[0] = new Vector2(1.3f, 0);  // Right
        ogreAttackPositions[1] = new Vector2(0, -1.3f); // Down
        ogreAttackPositions[2] = new Vector2(-1.3f, 0); // Left
        ogreAttackPositions[3] = new Vector2(0, 1.3f);  // Up
    }

    private void Update() {
        // Chase player or nah?
        if(this.HP > 0) {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if(distanceToPlayer < chaseRange){
                enemyState = EnemyState.CHASING;
            }
            else{
                enemyState = EnemyState.PATROLLING;
            }

            // Close enough to attack?
            if(distanceToPlayer < attackRange){
                if(!isAttacking){
                    StartCoroutine("AttackCoroutine");
                }
            }

            if(enemyState == EnemyState.PATROLLING && !knockedBack && !isAttacking){
                // Move towards goalPos
                float distance = ((Vector2)transform.position - goalPos).magnitude; 
                if(distance > 0.1f) {
                    MoveRigidbodyTowards(goalPos, patrollingSpeed);
                }
                else{
                    goalPos = GetRandomPointInsideZone();
                }
            }
            else if(enemyState == EnemyState.CHASING && !knockedBack && !isAttacking){
                // Chase the player!
                float distance = (playerTransform.position - transform.position).magnitude;
                if(distance > 0.1f){
                    MoveRigidbodyTowards((Vector2)playerTransform.position, chasingSpeed);
                }
            }

            // Animation directionality
            Vector2 velocityVector = rBody.velocity.normalized;
            animator.SetFloat("x", velocityVector.x);
            animator.SetFloat("y", velocityVector.y);

            if(enemyState == EnemyState.CHASING) {
                if(playerTransform.position.x > transform.position.x){
                    spre.flipX = false;
                }
                else{
                    spre.flipX = true;
                }
            }
            else{
                if(velocityVector.x > 0){
                    spre.flipX = false;
                }
                else{
                    spre.flipX = true;
                }
            }
        }
    }

    public override void DungeonReset(){
        isAttacking = false;
        StopAllCoroutines();
    }

    public override void TakeDamage(int howMuch, float knockBack){
        HP-=howMuch;

        // Spawn blooderino
        FindObjectOfType<GameMaster>().SpawnSlashBlood(transform.position);

        // knockback away from the player
        StopCoroutine("GetKnockedBack");
        StartCoroutine(GetKnockedBack(knockBack*10*0.5f));

        // Did we die?
        if(HP <= 0){
            SoundEffectsSystem.PlaySFX("ogre_death");
            Die();
        }
    }

    private IEnumerator AttackCoroutine(){
        isAttacking = true;

        // SFX
        SoundEffectsSystem.PlaySFX("ogre_attack");

        // Check which direction we want to attack
        int bestIndex = 0;
        for(int i = 1; i < 4; i++){
            float newDistance = Vector2.Distance((Vector2)transform.position + ogreAttackPositions[i], playerTransform.position);
            float bestDistance = Vector2.Distance((Vector2)transform.position + ogreAttackPositions[bestIndex], playerTransform.position);
            if(newDistance < bestDistance){
                bestIndex = i;
            }
        }

        animator.SetTrigger("attack_trigger");
        animator.SetFloat("attack_x", ogreAttackPositions[bestIndex].normalized.x);
        animator.SetFloat("attack_y", ogreAttackPositions[bestIndex].normalized.y);

        // Animaton wait time before actual attack
        // Attacking "wind-up" time
        yield return new WaitForSeconds(0.9f);

        // Perform attack!
        attackCollider.transform.localPosition = ogreAttackPositions[bestIndex];
        attackCollider.enabled = true;

        yield return new WaitForSeconds(0.4f);

        attackCollider.enabled = false;

        isAttacking = false;
    }

    private IEnumerator ChangeDirection(){
        while(true){
            yield return new WaitForSeconds(2.0f);
            goalPos = GetRandomPointInsideZone();
        }
    }
}
