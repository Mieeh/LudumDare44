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
    private GameObject playerGameObject;
    public GameObject dungeonGameObject;
    public GameObject shopGameObject;
    private GameObject UIScriptsGameObject;

    private Shop shopScript;

    public Image fadePanel;

    private void Awake() {
        playerGameObject = FindObjectOfType<PlayerMove>().gameObject;
        UIScriptsGameObject = FindObjectOfType<InventoryUI>().gameObject; // InventoryUI & Shop scripts
        shopScript = FindObjectOfType<Shop>();
    }

    private void Start() {
        // Start the game by going to the dungeon
        GotoDungeon();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            GotoShop();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            GotoDungeon();
        }
    }

    public void GotoShop(){
        StopAllCoroutines();
        StartCoroutine("GotoShopCoroutine");
    }

    public void GotoDungeon(){
        StopAllCoroutines();
        StartCoroutine("GotoDungeonCoroutine");
    }

    private IEnumerator GotoShopCoroutine(){

        const float darkenSpeed = 1.5f;
        const float lightenSpeed = 1.5f;
        const float exitSpeed = 2.0f;

        // Disable the players stuff
        playerGameObject.GetComponent<PlayerMove>().enabled = false;
        playerGameObject.GetComponent<PlayerCombat>().enabled = false;

        // Darken the screen
        while(fadePanel.color != Color.black){
            fadePanel.color = Vector4.MoveTowards(fadePanel.color, Color.black, darkenSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // Disable player!
        playerGameObject.SetActive(false);

        // Disable dungeon
        dungeonGameObject.SetActive(false);
        // Enable shop
        shopGameObject.SetActive(true);

        // Positon the camera and player
        playerGameObject.transform.position = new Vector3(0,-2,0);
        Camera.main.transform.position = new Vector3(0,0,-10);

        // Now show the player again
        playerGameObject.SetActive(true);

        // Open the shop!
        shopScript.StartShop();

        // Lighten the screen again
        while(fadePanel.color != Color.clear){
            fadePanel.color = Vector4.MoveTowards(fadePanel.color, Color.clear, lightenSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator GotoDungeonCoroutine(){
        const float darkenSpeed = 1.5f;
        const float lightenSpeed = 1.5f;

        /*
        Player has PlayerMove & PlayerCombat disabled
        */

        // Darken the screen
        while(fadePanel.color != Color.black){
            fadePanel.color = Vector4.MoveTowards(fadePanel.color, Color.black, darkenSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // Disable the shop & enable the dungeon
        shopScript.CloseShop();
        shopGameObject.SetActive(false);
        dungeonGameObject.SetActive(true);

        // Reset all enemies!
        ResetAllEnemies();

        // Position the player & camera
        playerGameObject.transform.position = new Vector3(0, 4, 0);
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Enable player components
        playerGameObject.GetComponent<PlayerMove>().enabled = true;
        playerGameObject.GetComponent<PlayerCombat>().enabled = true;

        // Lightne the screen 
        while(fadePanel.color != Color.clear){
            fadePanel.color = Vector4.MoveTowards(fadePanel.color, Color.clear, lightenSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
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
