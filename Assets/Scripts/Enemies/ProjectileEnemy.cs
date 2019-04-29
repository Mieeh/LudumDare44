using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : EnemyBase
{
    public float patrolSpeed = 1.0f;
    public GameObject projectilePrefab;
    public float fireRateInSeconds = 1.0f; // 1/per second
    public float projectileSpeed = 4.0f;
    public float shootingRange = 4.0f;

    private Vector2 goalPos;

    private bool isShooting = false;

    private void Start(){
        BaseStart();

        StartCoroutine("ChangeDirection");

        goalPos = GetRandomPointInsideZone();
    }

    private void Update() {
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if(distanceToPlayer < shootingRange){
                if(!isShooting){
                    StartCoroutine("ShootAtPlayer");
                    isShooting = true;
                    animator.SetTrigger("attack_trigger");
                }
            }
            else{
                if(isShooting){
                    animator.SetTrigger("patrol_trigger");
                }
                StopCoroutine("ShootAtPlayer");
                isShooting = false;
            }
        }

        if(enemyState == EnemyState.PATROLLING && !knockedBack && !isShooting){
            float distance = ((Vector2)transform.position - goalPos).magnitude;
            if(distance > 0.1f){
                MoveRigidbodyTowards(goalPos, patrolSpeed);
            }
            else{
                goalPos = GetRandomPointInsideZone();
            }
        }

        // Animation directionality
        Vector2 velocityVector = rBody.velocity.normalized;
        animator.SetFloat("x", velocityVector.x);
        animator.SetFloat("y", velocityVector.y);

        Vector2 playerVector = (playerTransform.position - transform.position).normalized;
        animator.SetFloat("x_player", playerVector.x);
        animator.SetFloat("y_player", playerVector.y);
    }

    public override void TakeDamage(int howMuch, float knockBack){
        HP-=howMuch;

        // Spawn blooderino
        FindObjectOfType<GameMaster>().SpawnSlashBlood(transform.position);

        StopCoroutine("GetKnockedBack");
        StartCoroutine(GetKnockedBack(knockBack));

        if(HP <= 0){
            SoundEffectsSystem.PlaySFX("goblin_death");
            StopCoroutine("ShootAtPlayer");
            Die();
        }
    }

    private IEnumerator ShootAtPlayer(){
        while(true){
            yield return new WaitForSeconds(fireRateInSeconds);
            
            if(HP > 0){
                // SFX
                SoundEffectsSystem.PlaySFX("goblin_attack");

                // Shoot projectile!
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
                
                // Shoot the projectile towards the player
                Vector2 dir = playerTransform.position - transform.position;
                dir.Normalize();
                projectile.GetComponent<Rigidbody2D>().velocity = dir*projectileSpeed;

                // Make sure the projectile deals the correct amount of damage!
                projectile.GetComponent<Projectile>().damage = attack; 

                // Make sure the projectile is destroyed after some amount of time!
                Destroy(projectile, 6);
            }
        }
    }

    private IEnumerator ChangeDirection(){
        while(true){
            yield return new WaitForSeconds(2.0f);
            goalPos = GetRandomPointInsideZone();
        }
    }
    
}
