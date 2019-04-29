using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text signText;
    private string lastSignMessage;
    public GameObject signGameObject;
    [Header("Rope UI")]
    public GameObject ropeGiverGameObject;
    public TMP_Text ropeUseText;
    public TMP_Text ropeOutBlockText;
    public Image ropeImage;
    public GameObject ropeGiverWorldObject;
    public GameObject RelicUIGameObject;
    public GameObject InventoryUIObject;

    private PlayerCombat playerCombat;

    private bool hasRope = false;

    private void Awake() {
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Update() {
        healthText.text = playerCombat.HP.ToString();

        // Use rope
        if(Input.GetKeyDown(InputKeys.ESCAPE_KEY) && hasRope ){
            if(playerCombat.canEscape && playerCombat.HP > 0 && !InventoryUIObject.activeInHierarchy){
                hasRope = false;
                ropeUseText.gameObject.SetActive(false);
                ropeImage.gameObject.SetActive(false);
                ropeGiverGameObject.SetActive(false);
                FindObjectOfType<GameMaster>().GotoShop();
            }
            else{
                ropeOutBlockText.color = Color.white;
            }
        }

        // Rope acceptapance
        if(Input.GetKeyDown(InputKeys.INTERACT) && ropeGiverGameObject.activeInHierarchy){
            // Give rope to player
            // Remove 10% of life
            playerCombat.HP = (int)((playerCombat.HP * 0.9f)+0.5f);
            hasRope = true;
            ropeUseText.gameObject.SetActive(true);
            ropeImage.gameObject.SetActive(true);
            ropeGiverGameObject.SetActive(false);
        }

        if(hasRope){
            ropeGiverWorldObject.GetComponent<Collider2D>().enabled = (false);
        }
        else{
            ropeGiverWorldObject.GetComponent<Collider2D>().enabled = (true);
        }

        // Relic
        if(RelicUIGameObject.activeInHierarchy){
            if(Input.GetKeyDown(InputKeys.INTERACT) && playerCombat.HP >= 100000){
                RelicUIGameObject.SetActive(false);
                FindObjectOfType<GameMaster>().WinGame();
            }
        }

        // Fade out the "cant rope out right now text" constantly
        if(ropeOutBlockText.color != Color.clear){
            ropeOutBlockText.color = Color.Lerp(ropeOutBlockText.color, Color.clear, 1.0f*Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Sign"){
            signText.text = "";
            lastSignMessage = other.GetComponent<Sign>().message;
            StopCoroutine("SignCoroutine");
            StartCoroutine("SignCoroutine");
            signGameObject.SetActive(true);
        }
        if(other.tag == "RopeGiver"){
            ropeGiverGameObject.SetActive(true);
        }
        if(other.tag == "Relic"){
            RelicUIGameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Sign"){
            StopCoroutine("SignCoroutine");
            signGameObject.SetActive(false);
            signText.text = "";
        }
        if(other.tag == "RopeGiver"){
            ropeGiverGameObject.SetActive(false);
        }
        if(other.tag == "Relic"){
            RelicUIGameObject.SetActive(false);
        }
    }

    IEnumerator SignCoroutine(){

        yield return new WaitForSeconds(0.5f);

        const float signWaitTime = 0.045f;

        signText.text = "";
        foreach(char c in lastSignMessage){
            // SFX
            //SoundEffectsSystem.PlaySFX("dialogue_blip");

            signText.text += c;
            yield return new WaitForSeconds(signWaitTime);
        }
    }
}