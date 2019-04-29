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
    public GameObject playerUIGameObject;
    public GameObject SlashBloodPrefab, DeathBloodPrefab;
    public GameObject[] UIToDisableOnShop;
    private Toll toll;

    // Music themes
    public AudioClip dungeon_theme, shop_theme;
    private AudioSource audio_source;
    
    private Shop shopScript;

    public Image fadePanel;

    public TMPro.TMP_Text winText;

    private const float bgVolume = 0.5f;

    private void Awake() {
        playerGameObject = FindObjectOfType<PlayerMove>().gameObject;
        UIScriptsGameObject = FindObjectOfType<InventoryUI>().gameObject; // InventoryUI & Shop scripts
        shopScript = FindObjectOfType<Shop>();
        toll = FindObjectOfType<Toll>();
        audio_source = GetComponent<AudioSource>();
    }

    private void Start() {
        // Start the game by going to the dungeon
        StartCoroutine("ChangeToDungeonTheme");
        StartCoroutine("StartGame");
    }

    public void GotoShop(){
        StopAllCoroutines();
        StartCoroutine("ChangeToShopTheme");
        StartCoroutine("GotoShopCoroutine");
    }

    public void GotoDungeon(){
        StopAllCoroutines();
        StartCoroutine("ChangeToDungeonTheme");
        StartCoroutine("GotoDungeonCoroutine");
    }

    public void WinGame(){
        StartCoroutine("WinCoroutine");
    }

    private IEnumerator StartGame(){

        // Position the player & camera
        playerGameObject.transform.position = new Vector3(0, 4, 0);
        Camera.main.GetComponent<CameraFollow>().enabled = true;

        // Enable player components
        playerGameObject.GetComponent<PlayerMove>().enabled = true;
        playerGameObject.GetComponent<PlayerCombat>().enabled = true;
        playerGameObject.GetComponent<PlayerUI>().enabled = true;
        UIScriptsGameObject.GetComponent<InventoryUI>().enabled = true;
        playerGameObject.GetComponent<Animator>().enabled = true;
        playerUIGameObject.SetActive(true);

        while(fadePanel.color != Color.clear){
            fadePanel.color = Vector4.MoveTowards(fadePanel.color, Color.clear, 1.5f*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator WinCoroutine(){
        // Disable the players stuff
        playerGameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        playerGameObject.GetComponent<PlayerMove>().enabled = false;
        playerGameObject.GetComponent<PlayerCombat>().enabled = false;
        playerGameObject.GetComponent<PlayerUI>().enabled = false;
        playerGameObject.GetComponent<Animator>().enabled = false;
        playerGameObject.GetComponent<Collider2D>().enabled = false;
        playerGameObject.GetComponent<SpriteRenderer>().sprite = playerGameObject.GetComponent<PlayerCombat>().idleSprites[(int)PlayerCombat.AttackDirection.UP];
        UIScriptsGameObject.GetComponent<InventoryUI>().enabled = false;
        playerUIGameObject.SetActive(false);

        var spre = playerGameObject.GetComponent<SpriteRenderer>();

        while(spre.color != Color.yellow){
            spre.color = Vector4.MoveTowards(spre.color, Color.yellow, 0.6f*Time.deltaTime);
            playerGameObject.transform.Translate(Vector3.up*1.5f*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        while(spre.color != Color.clear){
            playerGameObject.transform.localScale = playerGameObject.transform.localScale + new Vector3(0.3f,0.3f,0)*Time.deltaTime;
            spre.color = Vector4.MoveTowards(spre.color, Color.clear, 0.6f*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        while(fadePanel.color != Color.black){
            fadePanel.color = Vector4.MoveTowards(fadePanel.color, Color.black, 1.5f*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        while(winText.color != Color.yellow){
            winText.color = Vector4.MoveTowards(winText.color, Color.yellow, 1.1f*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("title_screen");

    }

    private IEnumerator GotoShopCoroutine(){

        const float darkenSpeed = 0.7f;
        const float lightenSpeed = 0.7f;

        // Disable the players stuff
        playerGameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        playerGameObject.GetComponent<PlayerMove>().enabled = false;
        playerGameObject.GetComponent<PlayerCombat>().enabled = false;
        playerGameObject.GetComponent<PlayerUI>().enabled = false;
        playerGameObject.GetComponent<Animator>().enabled = false;
        playerGameObject.GetComponent<SpriteRenderer>().sprite = playerGameObject.GetComponent<PlayerCombat>().idleSprites[(int)PlayerCombat.AttackDirection.UP];
        UIScriptsGameObject.GetComponent<InventoryUI>().enabled = false;
        playerUIGameObject.SetActive(false);

        // Disable all sorts of ui
        foreach(var x in UIToDisableOnShop){
            x.SetActive(false);
        }
        toll.PlayerLeft();

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

        // Disable camera follow
        Camera.main.GetComponent<CameraFollow>().enabled = false;

        // Positon the camera and player
        playerGameObject.transform.position = new Vector3(0,-2,0);
        Camera.main.transform.position = new Vector3(0,1,-10);

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
        const float darkenSpeed = 0.7f;
        const float lightenSpeed = 0.8f;

        /*
        Player has PlayerMove & PlayerCombat disabled + inventoryUI + playerInventoryUI
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
        Camera.main.GetComponent<CameraFollow>().enabled = true;

        // Enable player components
        playerGameObject.GetComponent<PlayerMove>().enabled = true;
        playerGameObject.GetComponent<PlayerCombat>().enabled = true;
        playerGameObject.GetComponent<PlayerUI>().enabled = true;
        UIScriptsGameObject.GetComponent<InventoryUI>().enabled = true;
        playerGameObject.GetComponent<Animator>().enabled = true;
        playerUIGameObject.SetActive(true);

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

    public void GameOver() {
        // Disable everything neccesary, fade out, load title screen scene
        // SFX
        audio_source.Stop();
        SoundEffectsSystem.PlaySFX("game_over");
        StartCoroutine("GameOverCoroutine");
    }

    private IEnumerator GameOverCoroutine(){

        
        playerGameObject.GetComponent<Collider2D>().enabled = false;
        
        Destroy(playerGameObject.GetComponent<PlayerMove>());
        Destroy(playerGameObject.GetComponent<PlayerCombat>());
        playerGameObject.GetComponent<Rigidbody2D>().simulated = false;

        // Fade out the player
        SpriteRenderer playerSpre = playerGameObject.GetComponent<SpriteRenderer>();
        const float playerFadeSpeed = 0.3f;
        const float playerFloatSpeed = 0.6f;
        while(playerSpre.color != Color.clear){
            playerGameObject.transform.Translate(Vector3.up*playerFloatSpeed*Time.deltaTime);
            playerSpre.color = Vector4.MoveTowards(playerSpre.color, Color.clear, playerFadeSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // Fade panel
        const float darkenSpeed = 0.6f;
        while(fadePanel.color != Color.black){
            fadePanel.color = Vector4.MoveTowards(fadePanel.color, Color.black, darkenSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // See ya!
        SceneManager.LoadScene("title_screen");
    }

    public void SpawnSlashBlood(Vector3 pos){
        GameObject temp = Instantiate(SlashBloodPrefab, pos, Quaternion.identity);
        Destroy(temp, 3);
    }

    public void SpawnDeathBlood(Vector3 pos){
        GameObject temp = Instantiate(DeathBloodPrefab, pos, Quaternion.identity);
        Destroy(temp, 6);
    }

    IEnumerator ChangeToShopTheme(){
        
        const float fadeSpeed = 1.0f;
        
        while(audio_source.volume > 0){
            audio_source.volume = Mathf.MoveTowards(audio_source.volume, 0, fadeSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        audio_source.Stop();
        audio_source.clip = shop_theme;
        audio_source.Play();

        while(audio_source.volume < bgVolume){
            audio_source.volume = Mathf.MoveTowards(audio_source.volume, 1, fadeSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ChangeToDungeonTheme(){
        const float fadeSpeed = 1.0f;
        
        while(audio_source.volume > 0){
            audio_source.volume = Mathf.MoveTowards(audio_source.volume, 0, fadeSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        audio_source.Stop();
        audio_source.clip = dungeon_theme;
        audio_source.Play();

        while(audio_source.volume < bgVolume){
            audio_source.volume = Mathf.MoveTowards(audio_source.volume, 1, fadeSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

}
