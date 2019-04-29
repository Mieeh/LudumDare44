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

    private PlayerCombat playerCombat;

    private void Awake() {
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Update() {
        healthText.text = playerCombat.HP.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Sign"){
            lastSignMessage = other.GetComponent<Sign>().message;
            StartCoroutine("SignCoroutine");
            signGameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Sign"){
            StopCoroutine("SignCoroutine");
            signGameObject.SetActive(false);
            signText.text = "";
        }
    }

    IEnumerator SignCoroutine(){

        yield return new WaitForSeconds(0.8f);

        const float signWaitTime = 0.075f;

        signText.text = "";
        foreach(char c in lastSignMessage){
            // SFX
            SoundEffectsSystem.PlaySFX("dialogue_blip");

            signText.text += c;
            yield return new WaitForSeconds(signWaitTime);
        }
    }
}