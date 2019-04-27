using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
Controller of all things related to the game, alot of code will go here for convenience 
*/
public class GameMaster : MonoBehaviour
{
    private void Awake() {

    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            ResetAllEnemies();
        }
    }

    public void ResetAllEnemies(){
        // Find all enemies
        var allEnemies = FindObjectsOfType<EnemyBase>();

        // Reset them all!
        foreach(var enemy in allEnemies){
            enemy.gameObject.SetActive(true);
            enemy.ResetMe();
        }
    }
}
