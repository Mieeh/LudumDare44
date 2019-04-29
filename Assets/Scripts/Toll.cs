using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Toll : MonoBehaviour
{

    public int cost;
    public GameObject tollGameObject;
    public TMP_Text tollText;
    private PlayerCombat playerCombat;

    private bool playerInside = false;

    private void Start() {
        playerCombat = FindObjectOfType<PlayerCombat>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space) && playerInside){
            if(playerCombat.HP > cost){
                playerCombat.HP -= cost;
                StartCoroutine("OpenToll");
            }
            else{
                tollText.text += "\n<color=#AC3532Not enough!</color>";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<PlayerMove>() != null){
            PlayerEntered();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<PlayerMove>() != null){
            PlayerLeft();
        }
    }

    public void PlayerEntered(){
        playerInside = true;
        tollGameObject.SetActive(true);
        tollText.text = "Pay me <color=#AC3532>" + cost + "</color> to pass! \nPress <color=green>L</color> to pay";
    }
    public void PlayerLeft(){
        playerInside = false;
        tollGameObject.SetActive(false);
    }

    private IEnumerator OpenToll(){

        foreach(var x in GetComponents<Collider2D>()){
            x.enabled = false;
        }
        playerInside = false;

        SpriteRenderer spre = GetComponent<SpriteRenderer>();
        const float fadeSpeed = 1.0f;

        while(spre.color != Color.clear){
            spre.color = Color.Lerp(spre.color, Color.clear, fadeSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // Done
        Destroy(gameObject);
    }

}
