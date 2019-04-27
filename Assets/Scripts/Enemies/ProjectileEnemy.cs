using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : EnemyBase
{
    public float patrolSpeed = 1.0f;
    public GameObject projectilePrefab;
    public float fireRateInSeconds = 1.0f; // 1/per second
    public float projectileSpeed = 4.0f;

    private Vector2 goalPos;

    private void Start(){
        BaseStart();

        goalPos = GetRandomPointInsideZone();
    }

    private void Update() {
        if(enemyState == EnemyState.PATROLLING && !knockedBack){
            float distance = ((Vector2)transform.position - goalPos).magnitude;
            if(distance > 0.1f){
                MoveRigidbodyTowards(goalPos, patrolSpeed);
            }
            else{
                goalPos = GetRandomPointInsideZone();
            }
        }
    }

    public override void TakeDamage(int howMuch, float knockBack){
        HP-=howMuch;

        StopCoroutine("GetKnockedBack");
        StartCoroutine(GetKnockedBack(knockBack));

        if(HP <= 0)
            Die();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<PlayerCombat>() != null){
            enemyState = EnemyState.CHASING;
            StartCoroutine("ShootAtPlayer");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<PlayerCombat>() != null) {
            enemyState = EnemyState.PATROLLING;
            StopCoroutine("ShootAtPlayer");
        }
    }

    private IEnumerator ShootAtPlayer(){
        while(true){
            yield return new WaitForSeconds(fireRateInSeconds);

            // Shoot projectile!
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
            
            // Shoot the projectile towards the player
            Vector2 dir = playerTransform.position - transform.position;
            dir.Normalize();
            projectile.GetComponent<Rigidbody2D>().velocity = dir*projectileSpeed;

            // Make sure the projectile is destroyed after some amount of time!
            Destroy(projectile, 6);
        }
    }
    
}
