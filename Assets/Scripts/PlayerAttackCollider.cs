using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private CameraFollow cam;

    private void Awake() {
        cam = FindObjectOfType<CameraFollow>();
        playerCombat = GetComponentInParent<PlayerCombat>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // @ Knockback factor is based on what???
        if(other.GetComponent<EnemyBase>() != null && !other.isTrigger){
            other.GetComponent<EnemyBase>().TakeDamage(playerCombat.GetPlayerAttack(), 400f);
        }
        if(other.tag == "LootCrate"){
            other.GetComponent<LootCrate>().GetDestroyed();
            cam.LightShake();
        }
    }
}
